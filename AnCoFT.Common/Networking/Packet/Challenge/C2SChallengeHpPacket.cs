using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Challenge
{
    public class C2SChallengeHpPacket : Packet
    {
        public C2SChallengeHpPacket(Packet packet)
            : base(packet)
        {
            this.NpcHp = this.ReadShort();
            this.PlayerHp = this.ReadShort();
        }

        public short NpcHp { get; set; }
        public short PlayerHp { get; set; }
    }
}
