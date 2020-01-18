using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildChangeSubMasterRequestPacket : Packet
    {
        public C2SGuildChangeSubMasterRequestPacket(Packet packet)
            : base(packet)
        {
            this.Status = this.ReadByte();
            this.CharacterId = this.ReadInteger();
        }

        public byte Status { get; set; }
        public int CharacterId { get; set; }
    }
}
