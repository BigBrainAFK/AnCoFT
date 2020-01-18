using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomListAnswerPacket : Packet
    {
        public S2CRoomListAnswerPacket(List<Game.MatchPlay.Room.Room> roomList)
            : base(Networking.Packet.PacketId.S2CRoomListAnswer)
        {
            this.Write((short)roomList.Count);

            for (int i = 0; i < roomList.Count; i++)
            {
                this.Write((short)i);
                this.Write(roomList[i].Name);
                this.Write(roomList[i].GameMode);
                this.Write(roomList[i].BattleMode);
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write(0); // Unknown
                this.Write((int)roomList[i].Ball);
                this.Write(roomList[i].MaxPlayer);
                this.Write(roomList[i].Private);
                this.Write(roomList[i].Level);
                this.Write(roomList[i].LevelRange);
                this.Write((byte)0); // Unknown
                this.Write(roomList[i].Map);
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)roomList[i].CurrentPlayers.Count); // Unknown
                this.Write((byte)0); // Unknown
                this.Write((byte)0); // Unknown
            }
        }
    }
}
