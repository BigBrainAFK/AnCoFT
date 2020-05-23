using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class C2SCharacterDeleteRequestPacket : Packet
    {
        public C2SCharacterDeleteRequestPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }
}
