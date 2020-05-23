namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildListRequestPacket : Packet
    {
        public C2SGuildListRequestPacket(Packet packet)
            : base(packet)
        {
            this.Page = this.ReadInteger();
        }

        public int Page { get; set; }
    }
}
