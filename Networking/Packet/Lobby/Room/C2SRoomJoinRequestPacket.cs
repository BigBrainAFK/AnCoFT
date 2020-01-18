using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class C2SRoomJoinRequestPacket : Packet
    {
        public C2SRoomJoinRequestPacket(Packet packet)
            : base(packet)
        {
            this.RoomId = this.ReadShort();
        }

        public short RoomId { get; set; }
    }
}
