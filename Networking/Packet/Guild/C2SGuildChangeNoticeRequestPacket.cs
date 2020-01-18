using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildChangeNoticeRequestPacket : Packet
    {
        public C2SGuildChangeNoticeRequestPacket(Packet packet)
            : base(packet)
        {
            this.Notice = this.ReadUnicodeString();
        }

        public string Notice { get; set; }
    }
}
