namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildChangeNoticeAnswerPacket : Packet
    {
        public S2CGuildChangeNoticeAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildChangeNoticeAnswer)
        {
            this.Write(result);
        }
    }
}
