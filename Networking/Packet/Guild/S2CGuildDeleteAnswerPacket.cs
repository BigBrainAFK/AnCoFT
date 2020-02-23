namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildDeleteAnswerPacket : Packet
    {
        public S2CGuildDeleteAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildDeleteAnswer)
        {
            this.Write(result);
        }
    }
}
