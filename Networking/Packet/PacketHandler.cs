namespace AnCoFT.Networking.Packet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using AnCoFT.Database.Models;
    using AnCoFT.Game.Item;
    using AnCoFT.Game.SinglePlay.Challenge;

    public class PacketHandler
    {
        public PacketHandler()
        {
        }

        public PacketHandler(GameHandler gameHandler)
        {
            this.GameHandler = gameHandler;
        }

        private GameHandler GameHandler { get; set; }

        public void HandlePacket(Client client, Packet packet)
        {
            switch (packet.PacketId)
            {
                case PacketId.C2SLoginRequest:
                    this.HandleLoginPacket(client, packet);
                    break;

                case PacketId.C2SDisconnectRequest:
                    this.HandleDisconnectPacket(client, packet);
                    break;

                case PacketId.C2SLoginFirstCharacterRequest:
                    this.HandleFirstCharacterPacket(client, packet);
                    break;

                case PacketId.C2SCharacterNameCheck:
                    this.HandleCharacterNameCheckPacket(client, packet);
                    break;

                case PacketId.C2SCharacterCreate:
                    this.HandleCharacterCreatePacket(client, packet);
                    break;

                case PacketId.C2SCharacterDelete:
                    this.HandleCharacterDeletePacket(client, packet);
                    break;

                case PacketId.C2SGameLoginData:
                    this.HandleGameServerLoginPacket(client, packet);
                    break;

                case PacketId.C2SGameReceiveData:
                    this.HandleGameServerDataRequestPacket(client, packet);
                    break;

                case PacketId.C2SShopApReq:
                    this.HandleShopMoneyRequestPacket(client, packet);
                    break;

                case PacketId.C2SChallengeProgressReq:
                    this.HandleChallengeProgressRequestPacket(client, packet);
                    break;

                case PacketId.C2STutorialProgressReq:
                    this.HandleTutorialProgressRequestPacket(client, packet);
                    break;

                case PacketId.C2SChallengeBeginReq:
                    this.HandleChallengeBeginRequestPacket(client, packet);
                    break;

                case PacketId.C2SChallengePoint:
                    this.HandleChallengePointPacket(client, packet);
                    break;

                case PacketId.C2SChallengeSet:
                    this.HandleChallengeSetPacket(client, packet);
                    break;

                case PacketId.C2SHeartbeat:
                case PacketId.C2SLoginAliveClient:
                    break;

                default:
                    this.HandleUnknown(client, packet);
                    break;
            }
        }

        public void SendWelcomePacket(Client client)
        {
            S2CWelcomePacket welcomePacket = new S2CWelcomePacket(0, 0, 0, 0);
            client.PacketStream.Write(welcomePacket);
        }

        public void HandleLoginPacket(Client client, Packet packet)
        {
            C2SLoginPacket loginPacket = new C2SLoginPacket(packet);

            Account account = client.DatabaseContext.Account
                .Include(a => a.Characters)?
                .First(a => a.Login == loginPacket.Username && a.Password == loginPacket.Password);

            if (account == null || account.AccountId <= 0)
            {
                S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.AccountInvalid);
                client.PacketStream.Write(loginAnswerPacket);
            }
            else
            {
                S2CLoginAnswerPacket loginAnswerPacket = new S2CLoginAnswerPacket(LoginResult.Success);
                client.PacketStream.Write(loginAnswerPacket);

                S2CCharacterListPacket characterListPacket = new S2CCharacterListPacket(account.AccountId, account.Characters);
                client.PacketStream.Write(characterListPacket);

                List<GameServer> gameServerList = client.DatabaseContext.GameServer.ToList();
                S2CGameServerListPacket gameServerListPacket = new S2CGameServerListPacket(gameServerList);
                client.PacketStream.Write(gameServerListPacket);
                client.Account = account;
            }
        }

        public void HandleDisconnectPacket(Client client, Packet packet)
        {
            S2CDisconnectAnswerPacket disconnectAnswerPacket = new S2CDisconnectAnswerPacket();
            client.PacketStream.Write(disconnectAnswerPacket);
        }

        public void HandleFirstCharacterPacket(Client client, Packet packet)
        {
            C2SFirstCharacterPacket firstCharacterPacket = new C2SFirstCharacterPacket(packet);

            if (client.Account.Characters?.Count == 0)
            {
                Character firstCharacter = new Character();
                firstCharacter.Type = firstCharacterPacket.CharacterType;
                client.DatabaseContext.Account.Include(a => a.Characters).First(a => a.AccountId == client.Account.AccountId).Characters.Add(firstCharacter);
                client.DatabaseContext.SaveChanges();

                S2CFirstCharacterAnswerPacket firstCharacterAnswerPacket = new S2CFirstCharacterAnswerPacket(0, firstCharacter.CharacterId, firstCharacter.Type);
                client.PacketStream.Write(firstCharacterAnswerPacket);
            }
            else
            {
                S2CFirstCharacterAnswerPacket firstCharacterAnswerPacket = new S2CFirstCharacterAnswerPacket(-1);
                client.PacketStream.Write(firstCharacterAnswerPacket);
            }
        }

        public void HandleCharacterNameCheckPacket(Client client, Packet packet)
        {
            C2SCharacterNameCheckPacket characterNameCheckPacket = new C2SCharacterNameCheckPacket(packet);
            if (client.DatabaseContext.Character.Count(c => c.Name == characterNameCheckPacket.Nickname) == 0)
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

        public void HandleCharacterCreatePacket(Client client, Packet packet)
        {
            C2SCharacterCreatePacket characterCreatePacket = new C2SCharacterCreatePacket(packet);

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

        public void HandleCharacterDeletePacket(Client client, Packet packet)
        {
            C2SCharacterDeletePacket characterDeletePacket = new C2SCharacterDeletePacket(packet);

            if (client.Account.Characters.Exists(c => c.CharacterId == characterDeletePacket.CharacterId))
            {
                client.DatabaseContext.Character.Remove(
                    client.Account.Characters.Find(c => c.CharacterId == characterDeletePacket.CharacterId));
                client.DatabaseContext.SaveChanges();

                S2CCharacterDeleteAnswerPacket characterDeleteAnswerPacket = new S2CCharacterDeleteAnswerPacket(0);
                client.PacketStream.Write(characterDeleteAnswerPacket);
                client.Account = client.DatabaseContext.Account.Include(a => a.Characters)
                    .First(a => a.AccountId == client.Account.AccountId);

                S2CCharacterListPacket characterListPacket = new S2CCharacterListPacket(client.Account.AccountId, client.Account.Characters);
                client.PacketStream.Write(characterListPacket);
            }
        }

        public void HandleGameServerDataRequestPacket(Client client, Packet packet)
        {
            C2SGameServerRequestPacket gameServerRequestPacket = new C2SGameServerRequestPacket(packet);
            S2CGameServerAnswerPacket gameServerAnswerPacket = new S2CGameServerAnswerPacket(gameServerRequestPacket.RequestType);
            client.PacketStream.Write(gameServerAnswerPacket);
        }

        public void HandleGameServerLoginPacket(Client client, Packet packet)
        {
            C2SGameServerLoginPacket gameServerLoginPacket = new C2SGameServerLoginPacket(packet);

            Account account = client.DatabaseContext.Account
                .Include(a => a.Characters)
                .AsEnumerable().Single(a => a.Characters.Exists(c => c.CharacterId == gameServerLoginPacket.CharacterId));

            if (account != null)
            {
                Packet gameServerLoginAnswer = new Packet(PacketId.S2CGameLoginData);
                gameServerLoginAnswer.Write((short)0, 1);
                client.PacketStream.Write(gameServerLoginAnswer);

                client.Account = account;
                client.ActiveCharacter = account.Characters.Find(c => c.CharacterId == gameServerLoginPacket.CharacterId);
            }
        }

        public void HandleShopMoneyRequestPacket(Client client, Packet packet)
        {
            Packet unknownAnswer = new Packet(0x1B61);
            unknownAnswer.Write(client.Account.Ap);
            unknownAnswer.Write(client.ActiveCharacter.Gold);
            client.PacketStream.Write(unknownAnswer);
        }

        public void HandleChallengeProgressRequestPacket(Client client, Packet packet)
        {
            List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress
                .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

            S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList);
            client.PacketStream.Write(challengeProgressAnswerPacket);
        }

        public void HandleTutorialProgressRequestPacket(Client client, Packet packet)
        {
            List<TutorialProgress> tutorialProgress = client.DatabaseContext.TutorialProgress
                .Where(t => t.CharacterId == client.Account.Characters[0].CharacterId).ToList();
            S2CTutorialProgressAnswerPacket tutorialProgressAnswerPacket = new S2CTutorialProgressAnswerPacket(tutorialProgress);
            client.PacketStream.Write(tutorialProgressAnswerPacket);
        }

        public void HandleChallengeBeginRequestPacket(Client client, Packet packet)
        {
            C2SChallengeBeginRequestPacket challengeBeginRequestPacket = new C2SChallengeBeginRequestPacket(packet);

            client.ActiveChallengeGame = new ChallengeBasicGame(challengeBeginRequestPacket.ChallengeId);

            Packet answer = new Packet(0x220D);
            answer.Write((short)1);
            client.PacketStream.Write(answer);
        }

        public void HandleChallengePointPacket(Client client, Packet packet)
        {
            C2SChallengePointPacket challengePointPacket = new C2SChallengePointPacket(packet);
            ((ChallengeBasicGame)client.ActiveChallengeGame).SetPoints(challengePointPacket.PointsPlayer, challengePointPacket.PointsNpc);

            if (((ChallengeBasicGame)client.ActiveChallengeGame).Finished)
            {
                bool win = ((ChallengeBasicGame)client.ActiveChallengeGame).SetsPlayer == 2;
                int timeNeeded = (((ChallengeBasicGame)client.ActiveChallengeGame).EndTime
                                  - ((ChallengeBasicGame)client.ActiveChallengeGame).StartTime).Seconds;

                S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(win, 1, 1, 2, timeNeeded, new List<ItemReward>());
                client.PacketStream.Write(challengeFinishPacket);

                ChallengeProgress challengeProgress = new ChallengeProgress();
                challengeProgress.CharacterId = client.ActiveCharacter.CharacterId;
                challengeProgress.ChallengeId = client.ActiveChallengeGame.ChallengeId;
                challengeProgress.Success += Convert.ToInt16(win);
                challengeProgress.Attempts += 1;
                client.DatabaseContext.ChallengeProgress.Add(challengeProgress);
                client.DatabaseContext.SaveChanges();
                client.ActiveChallengeGame = null;

                List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress
                    .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

                client.ActiveCharacter.ChallengeProgress = challengeProgressList;
                S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList);
                client.PacketStream.Write(challengeProgressAnswerPacket);
            }
        }

        public void HandleChallengeSetPacket(Client client, Packet packet)
        {
        }

        public void HandleUnknown(Client client, Packet packet)
        {
            Packet unknownAnswer = new Packet((ushort)(packet.PacketId + 1));
            unknownAnswer.Write((ushort)0);
            client.PacketStream.Write(unknownAnswer);
        }
    }
}