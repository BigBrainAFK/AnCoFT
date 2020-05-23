using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class C2SRoomMapChangeRequestPacket : Packet
    {
        public C2SRoomMapChangeRequestPacket(Packet packet)
            : base(packet)
        {
            this.Map = this.ReadByte();
        }

        public byte Map { get; set; }
    }
}
