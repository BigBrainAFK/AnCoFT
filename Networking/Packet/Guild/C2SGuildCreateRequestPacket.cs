namespace AnCoFT.Networking.Packet.Guild
{
    using System.Collections.Generic;

    public class C2SGuildCreateRequestPacket : Packet
    {
        public C2SGuildCreateRequestPacket(Packet packet)
            : base(packet)
        {
            this.Name = packet.ReadUnicodeString();
            this.Introduction = packet.ReadUnicodeString();
            this.Public = packet.ReadBoolean();
            this.LevelRestriction = packet.ReadByte();

            byte allowedCharacterCount = packet.ReadByte();
            this.AllowedCharacterType = new List<byte>();
            for (int i = 0; i < allowedCharacterCount; i++)
                this.AllowedCharacterType.Add(packet.ReadByte());
        }

        public string Name { get; set; }

        public string Introduction { get; set; }

        public bool Public { get; set; }

        public byte LevelRestriction { get; set; }

        public List<byte> AllowedCharacterType { get; set; }
    }
}
