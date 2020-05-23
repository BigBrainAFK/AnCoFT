namespace AnCoFT.Networking.Packet.Guild
{
    using AnCoFT.Game.Guild;

    public class S2CGuildChangeReverseMemberAnswerPacket : Packet
    {
        public S2CGuildChangeReverseMemberAnswerPacket(GuildMemberStatus status)
            : base(Networking.Packet.PacketId.S2CGuildChangeReverseMemberAnswer)
        {
            this.Write(status);
        }
    }
}
