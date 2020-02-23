namespace AnCoFT.Networking.Packet.Guild
{
    class S2CGuildJoinAnswerPacket : Packet
    {
        public S2CGuildJoinAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildJoinAnswer)
        {
            this.Write(result);
        }
    }
}
