namespace AnCoFT.Networking.Packet.Guild
{
    using System;
    using System.Collections.Generic;

    public class C2SGuildCreationRequestPacket : Packet
    {
        public C2SGuildCreationRequestPacket(Packet packet)
            : base(packet)
        {
            this.Name = packet.ReadUnicodeString();
            this.Description = packet.ReadUnicodeString();
            this.Public = Convert.ToBoolean(packet.ReadByte());
            this.MinLevel = packet.ReadByte();
            this.AllowedCharacterTypeCount = packet.ReadByte();
            this.AllowCharacterType = new List<byte>();

            for (int i = 0; i < this.AllowedCharacterTypeCount; i++)
                this.AllowCharacterType[i] = packet.ReadByte();
        }

        public string Name { get; }

        public string Description { get; }

        public bool Public { get; }

        public byte MinLevel { get; }

        public byte AllowedCharacterTypeCount { get; }

        public List<byte> AllowCharacterType { get; }
    }
}
