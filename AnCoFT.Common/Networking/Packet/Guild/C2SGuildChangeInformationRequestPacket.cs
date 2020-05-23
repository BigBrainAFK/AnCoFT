namespace AnCoFT.Networking.Packet.Guild
{
    using System;

    public class C2SGuildChangeInformationRequestPacket : Packet
    {
        public C2SGuildChangeInformationRequestPacket(Packet packet)
            : base(packet)
        {
            this.Introduction = packet.ReadUnicodeString();
            this.Public = Convert.ToBoolean(packet.ReadByte());
            this.MinLevel = packet.ReadByte();
            this.AllowedCharacterTypeCount = packet.ReadByte();
            this.AllowCharacterType = new byte[this.AllowedCharacterTypeCount];

            for (int i = 0; i < this.AllowedCharacterTypeCount; i++)
                this.AllowCharacterType[i] = packet.ReadByte();
        }

        public string Introduction { get; set; }

        public bool Public { get; }

        public byte MinLevel { get; }

        public byte AllowedCharacterTypeCount { get; }

        public byte[] AllowCharacterType { get; }
    }
}
