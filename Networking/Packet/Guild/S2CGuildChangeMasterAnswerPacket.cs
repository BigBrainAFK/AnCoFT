namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildChangeMasterAnswerPacket : Packet
    {
        public S2CGuildChangeMasterAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CGuildChangeMasterAnswer)
        {
            this.Write(result);
        }
    }
}
