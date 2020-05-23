using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Challenge
{
    public class C2SChallengeDamagePacket : Packet
    {
        public C2SChallengeDamagePacket(Packet packet)
            : base(packet)
        {
            this.Player = packet.ReadByte();
            this.Hp = packet.ReadInteger();
        }

        public byte Player { get; set; }

        public int Hp { get; set; }
    }
}
