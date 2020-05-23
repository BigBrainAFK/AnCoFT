using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class C2SCharacterNameCheckRequestPacket : Packet
    {
        public C2SCharacterNameCheckRequestPacket(Packet packet)
            : base(packet)
        {
            this.Nickname = this.ReadUnicodeString();
        }

        public string Nickname { get; }
    }
}
