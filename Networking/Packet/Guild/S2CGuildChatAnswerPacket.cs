namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildChatAnswerPacket : Packet
    {
        public S2CGuildChatAnswerPacket(string characterName, string message)
            : base(Networking.Packet.PacketId.S2CGuildChatAnswer)
        {
            this.Write(characterName);
            this.Write(message);
        }
    }
}
