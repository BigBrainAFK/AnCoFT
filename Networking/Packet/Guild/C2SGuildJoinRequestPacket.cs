namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildJoinRequestPacket : Packet
    {
        public C2SGuildJoinRequestPacket(Packet packet)
            : base(packet)
        {
            this.GuildId = this.ReadInteger();
        }

        public int GuildId { get; set; }
    }
}
