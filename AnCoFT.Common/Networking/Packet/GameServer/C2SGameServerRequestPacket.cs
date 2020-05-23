using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.GameServer
{
    public class C2SGameServerRequestPacket : Packet
    {
        public C2SGameServerRequestPacket(Packet packet)
            : base(packet)
        {
            this.RequestType = this.ReadByte();
        }

        public byte RequestType { get; set; }
    }
}
