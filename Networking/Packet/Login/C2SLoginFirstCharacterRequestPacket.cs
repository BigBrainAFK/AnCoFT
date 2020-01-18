using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class C2SLoginFirstCharacterRequestPacket : Packet
    {
        public C2SLoginFirstCharacterRequestPacket(Packet packet)
            : base(packet)
        {
            this.CharacterType = this.ReadByte();
        }

        public byte CharacterType { get; set; }
    }
}
