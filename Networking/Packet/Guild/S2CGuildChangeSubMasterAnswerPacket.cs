namespace AnCoFT.Networking.Packet.Guild
{
    using AnCoFT.Game.Guild;

    public class S2CGuildChangeSubMasterAnswerPacket : Packet
    {
        public S2CGuildChangeSubMasterAnswerPacket(GuildMemberStatus status)
            : base(Networking.Packet.PacketId.S2CGuildChangeSubMasterAnswer)
        {
            this.Write(status);
        }
    }
}