using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildChangeInformationRequestPacket : Packet
    {
        public C2SGuildChangeInformationRequestPacket(Packet packet)
            : base(packet)
        {
            this.Introduction = this.ReadUnicodeString();
            this.Public = Convert.ToBoolean(packet.ReadByte());
            this.MinLevel = packet.ReadByte();
            this.AllowedCharacterTypeCount = packet.ReadShort();
            this.AllowCharacterType = new List<byte>();

            for (int i = 0; i < this.AllowedCharacterTypeCount; i++)
                this.AllowCharacterType[i] = packet.ReadByte();
        }

        public string Introduction { get; set; }
        public bool Public { get; }
        public byte MinLevel { get; }
        public short AllowedCharacterTypeCount { get; }
        public List<byte> AllowCharacterType { get; }
    }
}
