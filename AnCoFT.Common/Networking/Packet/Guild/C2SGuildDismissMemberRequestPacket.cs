namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildDismissMemberRequestPacket : Packet
    {
        public C2SGuildDismissMemberRequestPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }
}
