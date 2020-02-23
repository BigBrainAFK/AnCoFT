namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildCreateAnswerPacket : Packet
    {
        public S2CGuildCreateAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildCreateAnswer)
        {
            this.Write(result);
        }
    }
}
