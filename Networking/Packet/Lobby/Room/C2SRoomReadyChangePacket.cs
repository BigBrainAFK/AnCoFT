using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class C2SRoomReadyChangePacket : Packet
    {
        public C2SRoomReadyChangePacket(Packet packet)
            : base(packet)
        {
            this.Ready = this.ReadByte();
        }

        public byte Ready { get; set; }
    }
}
