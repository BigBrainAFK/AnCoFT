using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Tutorial
{
    public class C2STutorialFinishRequestPacket : Packet
    {
        public C2STutorialFinishRequestPacket(Packet packet)
            : base(packet)
        {
            this.TutorialId = this.ReadByte();
        }

        public byte TutorialId { get; set; }
    }
}
