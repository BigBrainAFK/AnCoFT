namespace AnCoFT.Networking.Packet.Guild
{
    public class C2SGuildChangeMasterRequestPacket : Packet
    {
        public C2SGuildChangeMasterRequestPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
        }

        public int CharacterId { get; set; }
    }
}
