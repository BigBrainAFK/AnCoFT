namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildNameCheckAnswerPacket : Packet
    {
        public S2CGuildNameCheckAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildNameCheckAnswer)
        {
            this.Write(result);
        }
    }
}
