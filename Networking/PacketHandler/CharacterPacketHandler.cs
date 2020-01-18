using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Networking.Packet.Character;
using AnCoFT.Networking.Packet.Login;
using Microsoft.EntityFrameworkCore;

namespace AnCoFT.Networking.PacketHandler
{
    public static class CharacterPacketHandler
    {
        public static void HandleCharacterNameCheckPacket(Client client, Packet.Packet packet)
        {
            C2SCharacterNameCheckRequestPacket characterNameCheckPacket = new C2SCharacterNameCheckRequestPacket(packet);
            if (client.DatabaseContext.Character?.Count(c => c.Name == characterNameCheckPacket.Nickname) == 0)
            {
                S2CCharacterNameCheckAnswerPacket characterNameCheckAnswerPacket = new S2CCharacterNameCheckAnswerPacket(0);
                client.PacketStream.Write(characterNameCheckAnswerPacket);
            }
            else
            {
                S2CCharacterNameCheckAnswerPacket characterNameCheckAnswerPacket = new S2CCharacterNameCheckAnswerPacket(-1);
                client.PacketStream.Write(characterNameCheckAnswerPacket);
            }
        }

        public static void HandleCharacterCreatePacket(Client client, Packet.Packet packet)
        {
            C2SCharacterCreateRequestPacket characterCreatePacket = new C2SCharacterCreateRequestPacket(packet);

            Character character = client.DatabaseContext.Character.First(c => c.CharacterId == characterCreatePacket.CharacterId);
            character.Name = characterCreatePacket.Nickname;
            character.AlreadyCreated = true;
            character.Strength = characterCreatePacket.Strength;
            character.Stamina = characterCreatePacket.Stamina;
            character.Dexterity = characterCreatePacket.Dexterity;
            character.Willpower = characterCreatePacket.Willpower;
            character.StatusPoints = characterCreatePacket.StatusPoints;
            character.Type = characterCreatePacket.CharacterType;

            client.DatabaseContext.Character.Update(character);
            client.DatabaseContext.SaveChanges();

            S2CCharacterCreateAnswerPacket characterCreateAnswerPacket = new S2CCharacterCreateAnswerPacket(0);
            client.PacketStream.Write(characterCreateAnswerPacket);

            Account account = client.DatabaseContext.Account
                .Include(a => a.Characters)?
                .First(a => a.AccountId == client.Account.AccountId);
            client.Account = account;
        }

        public static void HandleCharacterDeletePacket(Client client, Packet.Packet packet)
        {
            C2SCharacterDeleteRequestPacket characterDeletePacket = new C2SCharacterDeleteRequestPacket(packet);

            if (client.Account.Characters.Exists(c => c.CharacterId == characterDeletePacket.CharacterId))
            {
                client.DatabaseContext.Character.Remove(
                    client.Account.Characters.Find(c => c.CharacterId == characterDeletePacket.CharacterId));
                client.DatabaseContext.SaveChanges();

                S2CCharacterDeleteAnswerPacket characterDeleteAnswerPacket = new S2CCharacterDeleteAnswerPacket(0);
                client.PacketStream.Write(characterDeleteAnswerPacket);
                client.Account = client.DatabaseContext.Account.Include(a => a.Characters)
                    .First(a => a.AccountId == client.Account.AccountId);

                S2CLoginCharacterListPacket characterListPacket = new S2CLoginCharacterListPacket(client.Account.AccountId, client.Account.Characters);
                client.PacketStream.Write(characterListPacket);
            }
        }

        public static void HandleCharacterMoneyRequestPacket(Client client, Packet.Packet packet)
        {
            S2CCharacterMoneyAnswerPacket characterMoneyAnswerPacket = new S2CCharacterMoneyAnswerPacket(client.Account.Ap, client.ActiveCharacter.Gold);
            client.PacketStream.Write(characterMoneyAnswerPacket);
        }
    }
}
