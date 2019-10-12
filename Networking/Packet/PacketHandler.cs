namespace AnCoFT.Networking.Packet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Microsoft.EntityFrameworkCore;

    using AnCoFT.Database.Models;
    using AnCoFT.Game.Item;
    using AnCoFT.Game.SinglePlay.Challenge;
    using AnCoFT.Game.MatchPlay.Room;

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

                case PacketId.C2SChallengeHp:
                    this.HandleChallengeHpPacket(client, packet);
                    break;

                case PacketId.C2SChallengePoint:
                    this.HandleChallengePointPacket(client, packet);
                    break;

                case PacketId.C2SChallengeDamage:
                    this.HandleChallengeDamagePacket(client, packet);
                    break;

                case PacketId.C2SChallengeSet:
                    this.HandleChallengeSetPacket(client, packet);
                    break;

                case PacketId.C2STutorialBegin:
                    this.HandleTutorialBeginPacket(client, packet);
                    break;

                case PacketId.C2STutorialEnd:
                    this.HandleTutorialEndPacket(client, packet);
                    break;

                case PacketId.C2SRoomCreate:
                    this.HandleRoomCreatePacket(client, packet);
                    break;

                case PacketId.C2SRoomPositionChange:
                    this.HandleRoomPositionChange(client, packet);
                    break;

                case PacketId.C2SRoomReadyChange:
                    this.HandleRoomReadyChange(client, packet);
                    break;

                case PacketId.C2SRoomMapChange:
                    this.HandleRoomMapChange(client, packet);
                    break;

                case PacketId.C2SRoomListReq:
                    this.HandleRoomListReqPacket(client, packet);
                    break;

                case PacketId.C2SLobbyUserListRequest:
                    this.HandleLobbyUserListReqPacket(client, packet);
                    break;

                case PacketId.C2SLobbyJoin:
                    this.HandleLobbyJoinLeave(client, true);
                    break;

                case PacketId.C2SLobbyLeave:
                    this.HandleLobbyJoinLeave(client, false);
                    break;

                case PacketId.C2SRoomJoin:
                    this.HandleRoomJoinPacket(client, packet);
                    break;

                case PacketId.C2SRoomStartGame:
                    this.HandleRoomStartGame(client, packet);
                    break;

                case 0x17DD:
                    this.Handle17DDPacket(client, packet);
                    break;

                case PacketId.C2SEmblemListRequest:
                    this.HandleEmblemListRequestPacket(client, packet);
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
            this.GameHandler?.RemoveClient(client);
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

            Challenge currentChallenge = this.GameHandler.GetChallenge(challengeBeginRequestPacket.ChallengeId);

            if (currentChallenge.GameMode == GameMode.Basic)
                client.ActiveChallengeGame = new ChallengeBasicGame(challengeBeginRequestPacket.ChallengeId);
            else if (currentChallenge.GameMode == GameMode.Battle)
                client.ActiveChallengeGame = new ChallengeBattleGame(challengeBeginRequestPacket.ChallengeId);

            Packet answer = new Packet(0x220D);
            answer.Write((short)1);
            client.PacketStream.Write(answer);
        }

        public void HandleChallengeHpPacket(Client client, Packet packet)
        {
            C2SChallengeHpPacket challengeHpPacket = new C2SChallengeHpPacket(packet);
            if (client.ActiveChallengeGame.GetType() == typeof(ChallengeBattleGame))
            {
                ((ChallengeBattleGame)client.ActiveChallengeGame).NpcHp = challengeHpPacket.NpcHp;
                ((ChallengeBattleGame)client.ActiveChallengeGame).PlayerHp = challengeHpPacket.PlayerHp;
            }
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

                var dbChallengeProgressContext = client.DatabaseContext.ChallengeProgress.Find(new object[]
                    {challengeProgress.CharacterId, challengeProgress.ChallengeId});

                if (dbChallengeProgressContext == null)
                    client.DatabaseContext.ChallengeProgress.Add(challengeProgress);
                else
                {
                    dbChallengeProgressContext.Success = challengeProgress.Success;
                    dbChallengeProgressContext.Attempts = challengeProgress.Attempts;
                    client.DatabaseContext.ChallengeProgress.Update(dbChallengeProgressContext);
                }

                client.DatabaseContext.SaveChanges();
                client.ActiveChallengeGame = null;

                List<ChallengeProgress> challengeProgressList = client.DatabaseContext.ChallengeProgress
                    .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

                client.ActiveCharacter.ChallengeProgress = challengeProgressList;
                S2CChallengeProgressAnswerPacket challengeProgressAnswerPacket = new S2CChallengeProgressAnswerPacket(challengeProgressList);
                client.PacketStream.Write(challengeProgressAnswerPacket);
            }
        }

        public void HandleChallengeDamagePacket(Client client, Packet packet)
        {
            C2SChallengeDamagePacket challengeDamagePacket = new C2SChallengeDamagePacket(packet);
            ((ChallengeBattleGame)client.ActiveChallengeGame).SetHp(challengeDamagePacket.Player,
                challengeDamagePacket.Hp);

            if (((ChallengeBattleGame)client.ActiveChallengeGame).Finished)
            {
                bool win = ((ChallengeBattleGame)client.ActiveChallengeGame).PlayerHp > 0;
                int timeNeeded = (((ChallengeBattleGame)client.ActiveChallengeGame).EndTime
                                  - ((ChallengeBattleGame)client.ActiveChallengeGame).StartTime).Seconds;

                S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(win, 1, 1, 2, timeNeeded, new List<ItemReward>());
                client.PacketStream.Write(challengeFinishPacket);

                ChallengeProgress challengeProgress = new ChallengeProgress();
                challengeProgress.CharacterId = client.ActiveCharacter.CharacterId;
                challengeProgress.ChallengeId = client.ActiveChallengeGame.ChallengeId;
                challengeProgress.Success += Convert.ToInt16(win);
                challengeProgress.Attempts += 1;

                var dbChallengeProgressContext = client.DatabaseContext.ChallengeProgress.Find(new object[]
                    {challengeProgress.CharacterId, challengeProgress.ChallengeId});

                if (dbChallengeProgressContext == null)
                    client.DatabaseContext.ChallengeProgress.Add(challengeProgress);
                else
                {
                    dbChallengeProgressContext.Success = challengeProgress.Success;
                    dbChallengeProgressContext.Attempts = challengeProgress.Attempts;
                    client.DatabaseContext.ChallengeProgress.Update(dbChallengeProgressContext);
                }

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

        public void HandleTutorialBeginPacket(Client client, Packet packet)
        {
        }

        public void HandleTutorialEndPacket(Client client, Packet packet)
        {
            C2STutorialEndPacket tutorialEndPacket = new C2STutorialEndPacket(packet);
            S2CChallengeFinishPacket challengeFinishPacket = new S2CChallengeFinishPacket(true, 1, 1, 2, 10, new List<ItemReward>());
            client.PacketStream.Write(challengeFinishPacket);

            TutorialProgress tutorialProgress = new TutorialProgress();
            tutorialProgress.CharacterId = client.ActiveCharacter.CharacterId;
            tutorialProgress.TutorialId = tutorialEndPacket.TutorialId;
            tutorialProgress.Success += Convert.ToInt16(1);
            tutorialProgress.Attempts += 1;

            var dbTutorialProgressContext = client.DatabaseContext.TutorialProgress.Find(new object[]
                {tutorialProgress.CharacterId, tutorialProgress.TutorialId});

            if (dbTutorialProgressContext == null)
                client.DatabaseContext.TutorialProgress.Add(tutorialProgress);
            else
            {
                dbTutorialProgressContext.Success = tutorialProgress.Success;
                dbTutorialProgressContext.Attempts = tutorialProgress.Attempts;
                client.DatabaseContext.TutorialProgress.Update(dbTutorialProgressContext);
            }

            client.DatabaseContext.SaveChanges();

            List<TutorialProgress> tutorialProgressList = client.DatabaseContext.TutorialProgress
                .Where(q => q.CharacterId == client.Account.Characters[0].CharacterId).ToList();

            client.ActiveCharacter.TutorialProgress = tutorialProgressList;
            S2CTutorialProgressAnswerPacket tutorialProgressAnswerPacket = new S2CTutorialProgressAnswerPacket(tutorialProgressList);
            client.PacketStream.Write(tutorialProgressAnswerPacket);
        }

        public void HandleRoomCreatePacket(Client client, Packet packet)
        {
            C2SRoomCreatePacket roomCreatePacket = new C2SRoomCreatePacket(packet);

            // Still needs fixes
            Room room = new Room();
            room.Id = Convert.ToInt16(this.GameHandler.Rooms.Count);
            room.Name = roomCreatePacket.Name;
            room.GameMode = 0;
            room.BattleMode = roomCreatePacket.GameMode;
            room.Level = client.ActiveCharacter.Level;
            room.LevelRange = roomCreatePacket.LevelRange;
            room.Map = 1;
            room.Betting = false;
            room.BettingCoins = 0;
            room.BettingGold = 0;
            room.MaxPlayer = roomCreatePacket.Players;

            RoomPlayer roomPlayer = new RoomPlayer();
            roomPlayer.Character = client.ActiveCharacter;
            roomPlayer.Position = 0;
            roomPlayer.Master = true;
            room.CurrentPlayers.Add(roomPlayer);

            this.GameHandler.Rooms.Add(room);

            client.ActiveRoom = room;

            Packet roomCreateAnswerPacket = new Packet(PacketId.S2CRoomCreateAnswer);
            roomCreateAnswerPacket.Write(0);
            client.PacketStream.Write(roomCreateAnswerPacket);

            S2CRoomInformation roomInformationPacket = new S2CRoomInformation(room);
            client.PacketStream.Write(roomInformationPacket);

            S2CRoomPlayerInformation roomPlayerInformationPacket = new S2CRoomPlayerInformation(room.CurrentPlayers);
            client.PacketStream.Write(roomPlayerInformationPacket);

            List<Client> clientsInLobby = this.GameHandler.GetClientsInLobby();
            S2CRoomListAnswerPacket roomListAnswerPacket = new S2CRoomListAnswerPacket(this.GameHandler.Rooms);
            foreach (Client c in clientsInLobby)
            {
                c.PacketStream.Write(roomListAnswerPacket);
            }
        }

        public void HandleRoomJoinPacket(Client client, Packet packet)
        {
            C2SRoomJoinPacket roomJoinPacket = new C2SRoomJoinPacket(packet);
            Room room = this.GameHandler.Rooms.Find(r => r.Id == roomJoinPacket.RoomId);

            S2CRoomJoinAnswer roomJoinAnswer = new S2CRoomJoinAnswer(0, 0, 0, 0);
            client.PacketStream.Write(roomJoinAnswer);
            client.ActiveRoom = room;

            S2CRoomInformation roomInformationPacket = new S2CRoomInformation(room);
            client.PacketStream.Write(roomInformationPacket);

            RoomPlayer roomPlayer = new RoomPlayer();
            roomPlayer.Character = client.ActiveCharacter;
            roomPlayer.Position = 1;
            roomPlayer.Master = false;
            room.CurrentPlayers.Add(roomPlayer);

            List<Client> clientsInRoom = this.GameHandler.GetClientsInRoom(room.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformation roomPlayerInformationPacket =
                    new S2CRoomPlayerInformation(room.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public void HandleRoomReadyChange(Client client, Packet packet)
        {
            C2SRoomReadyChangePacket roomReadyChangePacket = new C2SRoomReadyChangePacket(packet);
            client.ActiveRoom.CurrentPlayers.Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId)
                .Ready = Convert.ToBoolean(roomReadyChangePacket.Ready);

            List<Client> clientsInRoom = this.GameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformation roomPlayerInformationPacket =
                    new S2CRoomPlayerInformation(client.ActiveRoom.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public void HandleRoomPositionChange(Client client, Packet packet)
        {
            C2SRoomPositionChange roomPositionChangePacket = new C2SRoomPositionChange(packet);
            byte currentPosition = client.ActiveRoom.CurrentPlayers
                .Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId).Position;

            S2CRoomPositionChangeAnswer roomPositionChangeAnswer = new S2CRoomPositionChangeAnswer(0, currentPosition, roomPositionChangePacket.NewPosition);
            client.ActiveRoom.CurrentPlayers
                    .Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId).Position =
                Convert.ToByte(roomPositionChangePacket.NewPosition);
            client.PacketStream.Write(roomPositionChangeAnswer);

            List<Client> clientsInRoom = this.GameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformation roomPlayerInformationPacket =
                    new S2CRoomPlayerInformation(client.ActiveRoom.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public void HandleRoomMapChange(Client client, Packet packet)
        {
            C2SRoomMapChangePacket roomMapChangePacket = new C2SRoomMapChangePacket(packet);
            S2CRoomMapChangeAnswerPacket roomMapChangeAnswerPacket = new S2CRoomMapChangeAnswerPacket(roomMapChangePacket.Map);
            client.ActiveRoom.Map = roomMapChangePacket.Map;
            client.PacketStream.Write(roomMapChangeAnswerPacket);
        }

        public void HandleRoomStartGame(Client client, Packet packet)
        {
            List<Client> clientsInRoom = this.GameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                Packet testAnswer = new Packet(0x177C);
                testAnswer.Write((short)0);
                roomClient.PacketStream.Write(testAnswer);

                // Game Start Network Settings
                Packet testAnswer3 = new Packet(0x17DE);
                testAnswer3.Write((short)0);
                roomClient.PacketStream.Write(testAnswer3);

                Packet testAnswer2 = new Packet(0x17E6);
                testAnswer2.Write((short)0);
                roomClient.PacketStream.Write(testAnswer2);

                // Stats
                Packet testAnswer4 = new Packet(0x17E4);
                testAnswer4.Write((short)2);
                testAnswer4.Write((short)0);
                testAnswer4.Write("AnCoFT");
                testAnswer4.Write((short)0);
                testAnswer4.Write((byte)1);
                testAnswer4.Write(1);
                testAnswer4.Write(2);
                testAnswer4.Write(3);
                testAnswer4.Write((short)1);
                testAnswer4.Write("AnCoFT2");
                testAnswer4.Write((short)0);
                testAnswer4.Write((byte)1);
                testAnswer4.Write(1);
                testAnswer4.Write(2);
                testAnswer4.Write(3);
                roomClient.PacketStream.Write(testAnswer4);
            }
        }

        public void Handle17DDPacket(Client client, Packet packet)
        {
            /*
            Packet testAnswer = new Packet(0x17DE);
            testAnswer.Write((short)1);
            client.PacketStream.Write(testAnswer);
            */
        }

        public void HandleRoomListReqPacket(Client client, Packet packet)
        {
            // To Do: C2SRoomListReqPacket
            S2CRoomListAnswerPacket roomListAnswerPacket = new S2CRoomListAnswerPacket(this.GameHandler.Rooms);
            client.PacketStream.Write(roomListAnswerPacket);
        }

        public void HandleLobbyUserListReqPacket(Client client, Packet packet)
        {
            C2SLobbyUserListRequestPacket lobbyUserListRequestPacket = new C2SLobbyUserListRequestPacket(packet);
            List<Character> lobbyCharacterList = this.GameHandler.GetCharactersInLobby();
            S2CLobbyUserListAnswerPacket lobbyUserListAnswerPacket = new S2CLobbyUserListAnswerPacket(lobbyCharacterList);
            client.PacketStream.Write(lobbyUserListAnswerPacket);
        }

        public void HandleLobbyJoinLeave(Client client, bool joined)
        {
            if (joined)
                client.InLobby = true;
            else
                client.InLobby = false;
        }

        public void HandleEmblemListRequestPacket(Client client, Packet packet)
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