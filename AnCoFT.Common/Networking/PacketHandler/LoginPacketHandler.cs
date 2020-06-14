using System;
using System.Collections.Generic;
using System.Linq;
using AnCoFT.Database.Models;
using AnCoFT.Game.Login;
using AnCoFT.Networking.Packet.Login;
using Microsoft.EntityFrameworkCore;

namespace AnCoFT.Networking.PacketHandler
{
    public static class LoginPacketHandler
    {
        public static void HandleLoginPacket(Client client, Packet.Packet packet)
        {
            C2SLoginRequestPacket loginPacket = new C2SLoginRequestPacket(packet);

            Account account = new Account(DateTime.Now);

            try
            {
                account = client.DatabaseContext.Account
                    .Include(a => a.Characters)?
                    .First(a => a.Username == loginPacket.Username);
            }
            catch
            {
                S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.AccountInvalid);
                client.PacketStream.Write(loginAnswerPacket);
            }

            if (account == null || account.AccountId <= 0 || !BCrypt.Net.BCrypt.Verify(loginPacket.Password, account.Password))
            {
                S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.AccountInvalid);
                client.PacketStream.Write(loginAnswerPacket);
            }
			else if (!account.Enabled)
			{
				S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.AccountExpired);
				client.PacketStream.Write(loginAnswerPacket);
			}
			else if (account.Status == (int)LoginResult.AlreadyLoggedIn)
			{
				S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.AlreadyLoggedIn);
				client.PacketStream.Write(loginAnswerPacket);
			}
            else
            {
                S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.Success);
                client.PacketStream.Write(loginAnswerPacket);

                S2CLoginCharacterListPacket characterListPacket =
                    new S2CLoginCharacterListPacket(account.AccountId, account.Characters);
                client.PacketStream.Write(characterListPacket);

                List<GameServer> gameServerList = client.DatabaseContext.GameServer.ToList();
                S2CGameServerListPacket gameServerListPacket = new S2CGameServerListPacket(gameServerList);
                client.PacketStream.Write(gameServerListPacket);
                client.Account = account;

				//account.Status = (int)LoginResult.AlreadyLoggedIn;

				//client.DatabaseContext.Update(account);
				//client.DatabaseContext.SaveChanges(); // NEED TO REMOVE LATER AND IMPLEMENT PROPPER LOGOUT TIMING
            }
        }

        public static void HandleFirstCharacterPacket(Client client, Packet.Packet packet)
        {
            C2SLoginFirstCharacterRequestPacket firstCharacterPacket = new C2SLoginFirstCharacterRequestPacket(packet);

            if (client.Account.Characters?.Count == 0)
            {
                Character firstCharacter = new Character();
                firstCharacter.Type = firstCharacterPacket.CharacterType;
                client.DatabaseContext.Account.Include(a => a.Characters)
                    .First(a => a.AccountId == client.Account.AccountId).Characters.Add(firstCharacter);
                client.DatabaseContext.SaveChanges();

                S2CLoginFirstCharacterAnswerPacket firstCharacterAnswerPacket =
                    new S2CLoginFirstCharacterAnswerPacket(0, firstCharacter.CharacterId, firstCharacter.Type);
                client.PacketStream.Write(firstCharacterAnswerPacket);
            }
            else
            {
                S2CLoginFirstCharacterAnswerPacket firstCharacterAnswerPacket =
                    new S2CLoginFirstCharacterAnswerPacket(-1);
                client.PacketStream.Write(firstCharacterAnswerPacket);
            }
        }
    }
}
