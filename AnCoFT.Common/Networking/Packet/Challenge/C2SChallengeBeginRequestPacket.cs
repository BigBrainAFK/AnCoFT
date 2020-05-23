using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Challenge
{
    public class C2SChallengeBeginRequestPacket : Packet
    {
        public C2SChallengeBeginRequestPacket(Packet packet)
            : base(packet)
        {
            this.ChallengeId = packet.ReadShort();
        }

        public short ChallengeId { get; set; }
    }
}
