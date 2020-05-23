namespace AnCoFT.Networking.Packet.Guild
{
    using AnCoFT.Game.Guild;

    public class C2SGuildSearchRequestPacket : Packet
    {
        public C2SGuildSearchRequestPacket(Packet packet)
            : base(packet)
        {
            this.SearchType = (GuildSearchType)this.ReadByte();
            this.Number = this.ReadInteger();

            if (this.SearchType == GuildSearchType.Name)
                this.Name = this.ReadUnicodeString();
        }

        public GuildSearchType SearchType { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }
    }
}
