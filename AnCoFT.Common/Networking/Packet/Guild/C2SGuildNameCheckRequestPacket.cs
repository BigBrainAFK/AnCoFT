namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildNameCheckRequestPacket : Packet
    {
        public C2SGuildNameCheckRequestPacket(Packet packet)
            : base(packet)
        {
            this.GuildName = this.ReadUnicodeString();
        }

        public string GuildName { get; set; }
    }
}
