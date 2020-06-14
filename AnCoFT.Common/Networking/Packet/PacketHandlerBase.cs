using AnCoFT.Networking.Packet.Login;
using AnCoFT.Networking.PacketHandler;

namespace AnCoFT.Networking.Packet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Game.MatchPlay.Room;

    public class PacketHandlerBase
    {
        public PacketHandlerBase()
        {
        }

        public PacketHandlerBase(GameHandler gameHandler)
        {
            this.GameHandler = gameHandler;
        }

        private GameHandler GameHandler { get; set; }

        public void HandlePacket(Client client, Packet packet)
        {
            switch (packet.PacketId)
            {
                case PacketId.C2SLoginRequest:
                    LoginPacketHandler.HandleLoginPacket(client, packet);
                    break;

                case PacketId.C2SDisconnectRequest:
                    this.HandleDisconnectPacket(client, packet);
                    break;

                case PacketId.C2SLoginFirstCharacterRequest:
                    LoginPacketHandler.HandleFirstCharacterPacket(client, packet);
                    break;

                case PacketId.C2SCharacterNameCheck:
                    CharacterPacketHandler.HandleCharacterNameCheckPacket(client, packet);
                    break;

                case PacketId.C2SCharacterCreate:
                    CharacterPacketHandler.HandleCharacterCreatePacket(client, packet);
                    break;

                case PacketId.C2SCharacterDelete:
                    CharacterPacketHandler.HandleCharacterDeletePacket(client, packet);
                    break;

                case PacketId.C2SGameLoginData:
                    GameServerPacketHandler.HandleGameServerLoginPacket(client, packet);
                    break;

                case PacketId.C2SGameReceiveData:
                    GameServerPacketHandler.HandleGameServerDataRequestPacket(client, packet);
                    break;

                case PacketId.C2SCharacterMoneyRequest:
                    CharacterPacketHandler.HandleCharacterMoneyRequestPacket(client, packet);
                    break;

                case PacketId.C2SChallengeProgressReq:
                    ChallengePacketHandler.HandleChallengeProgressRequestPacket(client, packet);
                    break;

                case PacketId.C2SChallengeBeginReq:
                    ChallengePacketHandler.HandleChallengeBeginRequestPacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SChallengeHp:
                    ChallengePacketHandler.HandleChallengeHpPacket(client, packet);
                    break;

                case PacketId.C2SChallengePoint:
                    ChallengePacketHandler.HandleChallengePointPacket(client, packet);
                    break;

                case PacketId.C2SChallengeDamage:
                    ChallengePacketHandler.HandleChallengeDamagePacket(client, packet);
                    break;

                case PacketId.C2SChallengeSet:
                    ChallengePacketHandler.HandleChallengeSetPacket(client, packet);
                    break;

                case PacketId.C2STutorialProgressReq:
                    TutorialPacketHandler.HandleTutorialProgressRequestPacket(client, packet);
                    break;

                case PacketId.C2STutorialBegin:
                    TutorialPacketHandler.HandleTutorialBeginPacket(client, packet);
                    break;

                case PacketId.C2STutorialEnd:
                    TutorialPacketHandler.HandleTutorialEndPacket(client, packet);
                    break;

                case PacketId.C2SRoomCreate:
                    LobbyPacketHandler.HandleRoomCreatePacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SRoomPositionChange:
                    LobbyPacketHandler.HandleRoomPositionChange(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SRoomReadyChange:
                    LobbyPacketHandler.HandleRoomReadyChange(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SRoomMapChange:
                    LobbyPacketHandler.HandleRoomMapChange(client, packet);
                    break;

                case PacketId.C2SRoomListReq:
                    LobbyPacketHandler.HandleRoomListReqPacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SLobbyUserListRequest:
                    LobbyPacketHandler.HandleLobbyUserListReqPacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SLobbyJoin:
                    LobbyPacketHandler.HandleLobbyJoinLeave(client, true);
                    break;

                case PacketId.C2SLobbyChat:
                    LobbyPacketHandler.HandleLobbyChatPacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SLobbyLeave:
                    LobbyPacketHandler.HandleLobbyJoinLeave(client, false);
                    break;

                case PacketId.C2SRoomJoin:
                    LobbyPacketHandler.HandleRoomJoinPacket(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SRoomStartGame:
                    LobbyPacketHandler.HandleRoomStartGame(client, packet, this.GameHandler);
                    break;

                case PacketId.C2SHomeData:
                    HomePacketHandler.HandleHomeDataRequest(client, packet);
                    break;

                case PacketId.C2SHomeItemsLoadReq:
                    HomePacketHandler.HandleHomeInventoryRequest(client, packet);
                    break;

                case PacketId.C2SShopRequestDataPrepare:
                    ShopPacketHandler.HandleShopDataPrepare(client, packet);
                    break;

                case PacketId.C2SShopRequestData:
                    ShopPacketHandler.HandleShopData(client, packet);
                    break;

                case PacketId.C2SGuildListRequest:
                    GuildPacketHandler.HandleGuildListRequest(client, packet);
                    break;
/*
                case PacketId.C2SGuildDataRequest:
                    GuildPacketHandler.HandleGuildDataRequest(client, packet);
                    break;

                case PacketId.C2SGuildReserveMemberDataRequest:
                    GuildPacketHandler.HandleGuildReserveMemberDataRequest(client, packet);
                    break;

                case PacketId.C2SGuildMemberDataRequest:
                    GuildPacketHandler.HandleGuildMemberDataRequest(client, packet);
                    break;
*/
                case PacketId.C2SGuildGoldDataRequest:
                    GuildPacketHandler.HandleGuildGoldDataRequest(client, packet);
                    break;
/*
                case PacketId.C2SLeagueRankingRequest:
                    GuildPacketHandler.HandleGuildLeagueRankingRequest(client, packet);
                    break;
*/
                case PacketId.C2SMessengerFriendDataRequest:
                    MessengerPacketHandler.HandleMessengerFriendDataRequest(client, packet);
                    break;

                case PacketId.C2SMessengerFriendAddRequest:
                    MessengerPacketHandler.HandleMessengerFriendAddRequest(client, packet);
                    break;

                case PacketId.C2SMessengerFriendDeleteRequest:
                    MessengerPacketHandler.HandleMessengerFriendDeleteRequest(client, packet);
                    break;

                case PacketId.C2SMessengerProposalDataRequest:
                    MessengerPacketHandler.HandleMessengerProposalDataRequest(client, packet);
                    break;

                case PacketId.C2SMessengerParcelDataRequest:
                    MessengerPacketHandler.HandleMessengerParcelDataRequest(client, packet);
                    break;

                case PacketId.C2SMessengerMessageDataRequest:
                    MessengerPacketHandler.HandleMessengerMessageDataRequest(client, packet);
                    break;

                case PacketId.C2SMessengerMessageReadRequest:
                    MessengerPacketHandler.HandleMessengerMessageReadRequest(client, packet);
                    break;

                case PacketId.C2SMessengerMessageDeleteRequest:
                    MessengerPacketHandler.HandleMessengerMessageDeleteRequest(client, packet);
                    break;

                case PacketId.C2SQuestListRequest:
                    QuestPacketHandler.HandleEmblemListRequestPacket(client, packet);
                    break;

                case PacketId.C2SQuestAcceptRequest:
                    QuestPacketHandler.HandleQuestAcceptRequest(client, packet);
                    break;

                case PacketId.C2SHeartbeat:
                    this.HandleHeartbeat(client, packet);
                    break;

                case PacketId.C2SLoginAliveClient:
                    break;

                /*
                case 0x1071:
                    this.Handle1071(client, packet);
                    break;
                */
                case 0x0414:
                    this.Handle0414(client, packet);
                    break;

                case 0x18A5:
                    this.Handle18A5(client, packet);
                    break;

                case 0x17E1:
                    this.Handle17E1(client, packet);
                    break;

                case 0x17D9:
                    this.Handle17D9Packet(client, packet);
                    break;

                case 0x17DD:
                    this.Handle17DDPacket(client, packet);
                    break;

                case 0x3ED:
                    this.Handle03EDPacket(client, packet);
                    break;

                case 0x3F3:
                    this.Handle3f3Packet(client, packet);
                    break;

                case 0x1B6D:// Stat Apply Answer
					this.Handle1B6DPacket(client, packet);
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

        public void HandleDisconnectPacket(Client client, Packet packet)
        {
            this.GameHandler?.RemoveClient(client);
            S2CDisconnectAnswerPacket disconnectAnswerPacket = new S2CDisconnectAnswerPacket();
            client.PacketStream.Write(disconnectAnswerPacket);
        }

		public void HandleHeartbeat(Client client, Packet packet)
		{
			client.LastHeatbeat = DateTime.Now;
			// Create Heartbeat timeout loop so that clients who havent send a heartbeat since a minute timeout automatically
		}

        // 17E1 - Skip
        public void Handle17D9Packet(Client client, Packet packet)
        {
            Packet testPacket = new Packet(0x17DA);
            testPacket.Write((ushort)0);
            testPacket.Write("127.0.0.1");
            testPacket.Write((ushort)29999);
            testPacket.Write("dlrpajstkqwlfdldi");
            client.PacketStream.Write(testPacket);
        }

        public void HandleTestPacket(Client client, Packet packet)
        {
            bool test = false;
            if (test)
            {
                Packet testPacket3 = new Packet(0x2135);
                testPacket3.Write((byte)0x10);
                client.PacketStream.Write(testPacket3);
            }
        }

        public void Handle1071(Client client, Packet packet)
        {
            Packet testPacket = new Packet(0x1072);
            testPacket.Write(1);
            client.PacketStream.Write(testPacket);
        }
        public void Handle18A5(Client client, Packet packet)
        {
            Packet testPacket = new Packet(0x18A6);
            testPacket.Write((byte)0);
            testPacket.Write((byte)4);
            testPacket.Write(0);
            testPacket.Write((short)28);
            testPacket.Write(0);
            testPacket.Write(0);
            testPacket.Write(0);
            testPacket.Write(0);
            client.PacketStream.Write(testPacket);
        }
        public void Handle17E1(Client client, Packet packet)
        {
            // Player Ani
            Packet testPacket = new Packet(0x17E2);
            testPacket.Write((short)0);
            client.PacketStream.Write(testPacket);

            /*
            // Player Ani
            Packet testPacket2 = new Packet(0x17E3);
            testPacket2.Write((short)0);
            testPacket2.Write((short)0);
            testPacket2.Write((int)0);
            client.PacketStream.Write(testPacket2);
            */

            // Stats
            Packet testAnswer4 = new Packet(0x17E4);
            testAnswer4.Write((short)2);
            testAnswer4.Write((short)0);
            testAnswer4.Write("AnCoFT");
            testAnswer4.Write((byte)1);
            testAnswer4.Write(1);
            testAnswer4.Write(2);
            testAnswer4.Write(3);
            testAnswer4.Write((short)1);
            testAnswer4.Write("AnCoFT2");
            testAnswer4.Write((byte)1);
            testAnswer4.Write(1);
            testAnswer4.Write(2);
            testAnswer4.Write(3);
            client.PacketStream.Write(testAnswer4);
            
            // Start game result
            Packet testAnswer2 = new Packet(0x17E6);
            testAnswer2.Write((short)0);
            client.PacketStream.Write(testAnswer2);

            // Start?
            Packet testPacket2 = new Packet(0x183A);
            if (client.ActiveRoom.CurrentPlayers
                .Find(c => c.Character.CharacterId == client.ActiveCharacter.CharacterId).Master)
            {
                testPacket2.Write((short) 0);
                testPacket2.Write((short) 1);
                testPacket2.Write((short) 0);
            }
            else
            {
                testPacket2.Write((short) 1);
                testPacket2.Write((short) 0);
                testPacket2.Write((short) 1);
            }
            client.PacketStream.Write(testPacket2);

            // Start
            Packet testPacket5 = new Packet(0x183C);
            client.PacketStream.Write(testPacket5);

            // First Serve
            Packet testPacket3 = new Packet(0x183E);
            testPacket3.Write((short)1);
            if (client.ActiveRoom.CurrentPlayers
                .Find(c => c.Character.CharacterId == client.ActiveCharacter.CharacterId).Master)
            {
                testPacket3.Write((short)0);
                testPacket3.Write(1);
                testPacket3.Write(2);
                testPacket3.Write((byte)1);
            }
            else
            {
                testPacket3.Write((short)1);
                testPacket3.Write(1);
                testPacket3.Write(2);
                testPacket3.Write((byte)0);
            }
            client.PacketStream.Write(testPacket3);

            /*
            Packet testPacket3 = new Packet(0x183E);
            testPacket3.Write((short)0);
            client.PacketStream.Write(testPacket3);
            */
        }

        public void Handle1701(Client client, Packet packet)
        {

        }

        public void Handle0414(Client client, Packet packet)
        {
            Room room = this.GameHandler.Rooms.FirstOrDefault();
            List<Client> clientsInRoom = this.GameHandler.GetClientsInRoom(room.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                roomClient.PacketStream.Write(packet);
                client.PacketStream.Write(packet);
            }
        }

        public void Handle17DDPacket(Client client, Packet packet)
        {
            /*
            Packet testAnswer = new Packet(0x17DE);
            testAnswer.Write((short)2);
            client.PacketStream.Write(testAnswer);
            */
        }

        public void Handle03EDPacket(Client client, Packet packet)
        {
            Packet testAnswer = new Packet(0x03EF);
            testAnswer.Write((byte)0);
            client.PacketStream.Write(testAnswer);

            Packet testAnswer2 = new Packet(0x0403);
            testAnswer2.Write((byte)0);
            testAnswer2.Write(0);
            testAnswer2.Write(0);
            testAnswer2.Write(0);
            testAnswer2.Write(0);
            testAnswer2.Write(0);
            client.PacketStream.Write(testAnswer2);
        }

        public void Handle3f3Packet(Client client, Packet packet)
        {
            /*
            Packet testAnswer = new Packet(0x332D);
            testAnswer.Write((byte)0);
            client.PacketStream.Write(testAnswer);
            */
            /*
            Packet testAnswer = new Packet(0x3F7);
            testAnswer.Write("127.0.0.1");
            testAnswer.Write((short)5897);
            testAnswer.Write((short)0);
            testAnswer.Write((byte)0);
            testAnswer.Write((short)1);
            client.PacketStream.Write(testAnswer);

            Packet testAnswer5 = new Packet(0x403);
            testAnswer5.Write((byte)0);
            testAnswer5.Write(0);
            testAnswer5.Write(0);
            testAnswer5.Write(0);
            testAnswer5.Write(0);
            testAnswer5.Write(0);
            client.PacketStream.Write(testAnswer5);
            */

            /*
            Packet testAnswer6 = new Packet(0x416);
            client.PacketStream.Write(testAnswer6);
            */

        }

		// Stat Apply Answer
        public void Handle1B6DPacket(Client client, Packet packet)
        {
            Packet testAnswer = new Packet(0x1B6E);
			testAnswer.Write((byte)5); //???
			testAnswer.Write((byte)1); //???
			testAnswer.Write((byte)2); //???
			testAnswer.Write((byte)3); //???
            testAnswer.Write((byte)11); //str
            testAnswer.Write((byte)10); //sta
            testAnswer.Write((byte)10); //dex
            testAnswer.Write((byte)10); //will
            testAnswer.Write((byte)0); //str again?
            testAnswer.Write((byte)0); //sta again?
            testAnswer.Write((byte)0); //dex again?
            testAnswer.Write((byte)0); //will again?
			client.PacketStream.Write(testAnswer);
			//FIGURE OUT ATTRIBUTES
		}

        public void HandleWhisperPacket(Client client, Packet packet)
        {

        }

        public void HandleUnknown(Client client, Packet packet)
        {
            Console.WriteLine($"UNKNOWN: [{packet.PacketId:X4}] {BitConverter.ToString(packet.GetRawPacket(), 0, packet.DataLength + 8)}");
            Packet unknownAnswer = new Packet((ushort)(packet.PacketId + 1));
            unknownAnswer.Write((ushort)0);
            client.PacketStream.Write(unknownAnswer);
        }
    }
}
