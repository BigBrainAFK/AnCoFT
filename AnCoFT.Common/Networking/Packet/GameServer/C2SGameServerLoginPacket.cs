using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.GameServer
{
    public class C2SGameServerLoginPacket : Packet
    {
        public C2SGameServerLoginPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }
}
