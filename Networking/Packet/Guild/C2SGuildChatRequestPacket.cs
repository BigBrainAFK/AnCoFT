namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildChatRequestPacket : Packet
    {
        public C2SGuildChatRequestPacket(Packet packet)
            : base(packet)
        {
            this.Message = this.ReadUnicodeString();
        }

        public string Message { get; set; }
    }
}
