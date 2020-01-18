using System.Collections.Generic;
using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Game.Login;
using AnCoFT.Networking.Packet;
using AnCoFT.Networking.Packet.Character;
using AnCoFT.Networking.Packet.GameServer;
using AnCoFT.Networking.Packet.Home;
using AnCoFT.Networking.Packet.Login;
using Microsoft.EntityFrameworkCore;

namespace AnCoFT.Networking.PacketHandler
{
    public static class GameServerPacketHandler
    {

        public static void HandleGameServerDataRequestPacket(Client client, Packet.Packet packet)
        {
            C2SGameServerRequestPacket gameServerRequestPacket = new C2SGameServerRequestPacket(packet);

            switch (gameServerRequestPacket.RequestType)
            {
                // Lvl + Exp
                case 0:
                    S2CCharacterLevelExpData characterInfoPacket = new S2CCharacterLevelExpData(client.ActiveCharacter);
                    client.PacketStream.Write(characterInfoPacket);
                    break;
                // Home
                case 1:
                    Home tempHome = new Home();
                    tempHome.Level = 1;
                    tempHome.FurnitureCount = 0;
                    S2CHousingDataPacket housingDataPacket = new S2CHousingDataPacket(tempHome);
                    client.PacketStream.Write(housingDataPacket);
                    break;
                // Inventory
                case 2:
                    break;
            }
            S2CGameServerAnswerPacket gameServerAnswerPacket = new S2CGameServerAnswerPacket(gameServerRequestPacket.RequestType);
            client.PacketStream.Write(gameServerAnswerPacket);
        }

        public static void HandleGameServerLoginPacket(Client client, Packet.Packet packet)
        {
            C2SGameServerLoginPacket gameServerLoginPacket = new C2SGameServerLoginPacket(packet);

            Account account = client.DatabaseContext.Account
                .Include(a => a.Characters)
                .ThenInclude(c => c.GuildMember)
                .ThenInclude(gm => gm.Guild)
                .AsEnumerable().Single(a => a.Characters.Exists(c => c.CharacterId == gameServerLoginPacket.CharacterId));

            if (account != null)
            {
                Packet.Packet gameServerLoginAnswer = new Packet.Packet(PacketId.S2CGameLoginData);
                gameServerLoginAnswer.Write((short)0, 1);
                client.PacketStream.Write(gameServerLoginAnswer);

                client.Account = account;
                client.ActiveCharacter = account.Characters.Find(c => c.CharacterId == gameServerLoginPacket.CharacterId);
                client.ActiveCharacter.MessengerFriendList = client.DatabaseContext.MessengerFriendList?
                    .Where(f => f.CharacterId == client.ActiveCharacter.CharacterId).ToList() ?? new List<MessengerFriend>();
            }
        }
    }
}