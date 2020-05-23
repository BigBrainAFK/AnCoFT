using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Challenge
{
    public class C2SChallengePointPacket : Packet
    {
        public C2SChallengePointPacket(Packet packet)
            : base(packet)
        {
            this.PointsPlayer = packet.ReadByte();
            this.PointsNpc = packet.ReadByte();
        }

        public byte PointsPlayer { get; set; }

        public byte PointsNpc { get; set; }
    }
}
