using System;
using System.Collections.Generic;
using AnCoFT.Game.Chat;
using AnCoFT.Game.MatchPlay.Room;
using AnCoFT.Networking.Packet;
using AnCoFT.Networking.Packet.Lobby;
using AnCoFT.Networking.Packet.Lobby.Room;

namespace AnCoFT.Networking.PacketHandler
{
    public static class LobbyPacketHandler
    {
        public static void HandleRoomCreatePacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SRoomCreatePacket roomCreatePacket = new C2SRoomCreatePacket(packet);

            // Still needs fixes
            Room room = new Room();
            room.Id = Convert.ToInt16(gameHandler.Rooms.Count);
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

            gameHandler.Rooms.Add(room);

            client.ActiveRoom = room;

            Packet.Packet roomCreateAnswerPacket = new Packet.Packet(PacketId.S2CRoomCreateAnswer);
            roomCreateAnswerPacket.Write(0);
            client.PacketStream.Write(roomCreateAnswerPacket);

            S2CRoomInformationPacket roomInformationPacket = new S2CRoomInformationPacket(room);
            client.PacketStream.Write(roomInformationPacket);

            S2CRoomPlayerInformationPacket roomPlayerInformationPacket = new S2CRoomPlayerInformationPacket(room.CurrentPlayers);
            client.PacketStream.Write(roomPlayerInformationPacket);

            List<Client> clientsInLobby = gameHandler.GetClientsInLobby();
            S2CRoomListAnswerPacket roomListAnswerPacket = new S2CRoomListAnswerPacket(gameHandler.Rooms);
            foreach (Client c in clientsInLobby)
            {
                c.PacketStream.Write(roomListAnswerPacket);
            }
        }

        public static void HandleRoomJoinPacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SRoomJoinRequestPacket roomJoinPacket = new C2SRoomJoinRequestPacket(packet);
            Room room = gameHandler.Rooms.Find(r => r.Id == roomJoinPacket.RoomId);

            S2CRoomJoinAnswerPacket roomJoinAnswer = new S2CRoomJoinAnswerPacket(0, 0, 0, 0);
            client.PacketStream.Write(roomJoinAnswer);
            client.ActiveRoom = room;

            S2CRoomInformationPacket roomInformationPacket = new S2CRoomInformationPacket(room);
            client.PacketStream.Write(roomInformationPacket);

            RoomPlayer roomPlayer = new RoomPlayer();
            roomPlayer.Character = client.ActiveCharacter;
            roomPlayer.Position = 1;
            roomPlayer.Master = false;
            room.CurrentPlayers.Add(roomPlayer);

            List<Client> clientsInRoom = gameHandler.GetClientsInRoom(room.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformationPacket roomPlayerInformationPacket =
                    new S2CRoomPlayerInformationPacket(room.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public static void HandleRoomReadyChange(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SRoomReadyChangePacket roomReadyChangePacket = new C2SRoomReadyChangePacket(packet);
            client.ActiveRoom.CurrentPlayers.Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId)
                .Ready = Convert.ToBoolean(roomReadyChangePacket.Ready);

            List<Client> clientsInRoom = gameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformationPacket roomPlayerInformationPacket =
                    new S2CRoomPlayerInformationPacket(client.ActiveRoom.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public static void HandleRoomPositionChange(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SRoomPositionChangeRequestPacket roomPositionChangePacket = new C2SRoomPositionChangeRequestPacket(packet);
            byte currentPosition = client.ActiveRoom.CurrentPlayers
                .Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId).Position;

            S2CRoomPositionChangeAnswerPacket roomPositionChangeAnswer = new S2CRoomPositionChangeAnswerPacket(0, currentPosition, roomPositionChangePacket.NewPosition);
            client.ActiveRoom.CurrentPlayers
                    .Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId).Position =
                Convert.ToByte(roomPositionChangePacket.NewPosition);
            client.PacketStream.Write(roomPositionChangeAnswer);

            List<Client> clientsInRoom = gameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                S2CRoomPlayerInformationPacket roomPlayerInformationPacket =
                    new S2CRoomPlayerInformationPacket(client.ActiveRoom.CurrentPlayers);
                roomClient.PacketStream.Write(roomPlayerInformationPacket);
            }
        }

        public static void HandleRoomMapChange(Client client, Packet.Packet packet)
        {
            C2SRoomMapChangeRequestPacket roomMapChangePacket = new C2SRoomMapChangeRequestPacket(packet);
            S2CRoomMapChangeAnswerPacket roomMapChangeAnswerPacket = new S2CRoomMapChangeAnswerPacket(roomMapChangePacket.Map);
            client.ActiveRoom.Map = roomMapChangePacket.Map;
            client.PacketStream.Write(roomMapChangeAnswerPacket);
        }

        public static void HandleLobbyChatPacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SLobbyChatRequestPacket lobbyChatPacket = new C2SLobbyChatRequestPacket(packet);
            S2CLobbyChatAnswerPacket lobbyChatAnswerAnswerPacket = new S2CLobbyChatAnswerPacket(lobbyChatPacket.Type,
                client.ActiveCharacter.Name, lobbyChatPacket.Message);
            switch (lobbyChatPacket.Type)
            {
                case ChatType.All:
                {
                    List<Client> clientsInLobby = gameHandler.GetClientsInLobby();
                    foreach (Client c in clientsInLobby)
                        c.PacketStream.Write(lobbyChatAnswerAnswerPacket);
                }
                    break;
                case ChatType.Club:
                    break;
            }
        }

        public static void HandleRoomListReqPacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            // To Do: C2SRoomListReqPacket
            S2CRoomListAnswerPacket roomListAnswerPacket = new S2CRoomListAnswerPacket(gameHandler.Rooms);
            client.PacketStream.Write(roomListAnswerPacket);
        }

        public static void HandleLobbyUserListReqPacket(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            C2SLobbyUserListRequestPacket lobbyUserListRequestPacket = new C2SLobbyUserListRequestPacket(packet);
            List<Database.Models.Character> lobbyCharacterList = gameHandler.GetCharactersInLobby();
            S2CLobbyUserListAnswerPacket lobbyUserListAnswerPacket = new S2CLobbyUserListAnswerPacket(lobbyCharacterList);
            client.PacketStream.Write(lobbyUserListAnswerPacket);
        }

        public static void HandleLobbyJoinLeave(Client client, bool joined)
        {
            if (joined)
                client.InLobby = true;
            else
                client.InLobby = false;
        }

        public static void HandleRoomStartGame(Client client, Packet.Packet packet, GameHandler gameHandler)
        {
            List<Client> clientsInRoom = gameHandler.GetClientsInRoom(client.ActiveRoom.Id);
            foreach (Client roomClient in clientsInRoom)
            {
                bool master = roomClient.ActiveRoom.CurrentPlayers
                    .Find(rp => rp.Character.CharacterId == client.ActiveCharacter.CharacterId).Master;

                Packet.Packet testAnswer = new Packet.Packet(0x177C);
                testAnswer.Write((short)0);
                roomClient.PacketStream.Write(testAnswer);

                /*
                // Udp Server
                Packet testAnswer5 = new Packet(0x177E);
                testAnswer5.Write((byte)0);
                testAnswer5.Write((ushort)52080);
                testAnswer5.Write("127.0.0.1");
                testAnswer5.Write((ushort)52080);
                client.PacketStream.Write(testAnswer5);
                */

                Packet.Packet testPacket99 = new Packet.Packet(0x3EA);
                testPacket99.Write("127.0.0.1");
                testPacket99.Write((ushort)5896);
                testPacket99.Write(1);
                testPacket99.Write(2);
                testPacket99.Write(0);
                testPacket99.Write(0);
                roomClient.PacketStream.Write(testPacket99);

                /*
                // idk - Peers?
                Packet testAnswer6 = new Packet(0x17D8);
                testAnswer6.Write((short)1);
                testAnswer6.Write("127.0.0.1");
                roomClient.PacketStream.Write(testAnswer6);
                */

                /*
                // TCP Relay Server?
                Packet testPacket99 = new Packet(0x3EA);
                testPacket99.Write("127.0.0.1");
                testPacket99.Write((ushort)5896);
                testPacket99.Write(0);
                testPacket99.Write(0);
                testPacket99.Write(0);
                testPacket99.Write(0);
                roomClient.PacketStream.Write(testPacket99);
                */

                // Game Start Network Settings
                Packet.Packet testAnswer3 = new Packet.Packet(0x17DE);
                testAnswer3.Write((short)0);
                roomClient.PacketStream.Write(testAnswer3);

                Packet.Packet testPacket = new Packet.Packet(0x17E0);
                roomClient.PacketStream.Write(testPacket);
            }
        }
    }
}
