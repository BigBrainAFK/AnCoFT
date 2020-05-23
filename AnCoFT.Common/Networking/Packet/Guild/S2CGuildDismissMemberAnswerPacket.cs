namespace AnCoFT.Networking.Packet.Guild
{
    class S2CGuildDismissMemberAnswerPacket : Packet
    {
        public S2CGuildDismissMemberAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildDismissMemberAnswer)
        {
            this.Write(result);
        }
    }
}
