namespace AnCoFT.Networking.Packet.Guild
{
    using AnCoFT.Game.Guild;

    public class C2SGuildChangeReverseMemberRequestPacket : Packet
    {
        public C2SGuildChangeReverseMemberRequestPacket(Packet packet)
            : base(packet)
        {
            this.MemberId = this.ReadByte();
            this.Status = (GuildMemberStatus)this.ReadByte();
            this.CharacterId = this.ReadInteger();
        }

        public byte MemberId { get; set; }

        public GuildMemberStatus Status { get; set; }

        public int CharacterId { get; set; }
    }
}
