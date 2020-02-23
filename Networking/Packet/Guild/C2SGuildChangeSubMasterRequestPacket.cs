namespace AnCoFT.Networking.Packet.Guild
{
    using AnCoFT.Game.Guild;

    public class C2SGuildChangeSubMasterRequestPacket : Packet
    {
        public C2SGuildChangeSubMasterRequestPacket(Packet packet)
            : base(packet)
        {
            this.Status = (GuildMemberStatus)this.ReadByte();
            this.CharacterId = this.ReadInteger();
        }

        public GuildMemberStatus Status { get; set; }

        public int CharacterId { get; set; }
    }
}
