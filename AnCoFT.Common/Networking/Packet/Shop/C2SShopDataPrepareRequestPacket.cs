using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Shop
{
    public class C2SShopDataPrepareRequestPacket : Packet
    {
        public C2SShopDataPrepareRequestPacket(Packet packet)
            : base(packet)
        {
            this.Category = packet.ReadByte();
            this.Part = packet.ReadByte();
            this.Character = packet.ReadByte();
        }

        public byte Category;
        public byte Part;
        public byte Character;
    }
}
