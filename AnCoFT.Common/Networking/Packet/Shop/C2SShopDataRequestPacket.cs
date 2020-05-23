using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Shop
{
    public class C2SShopDataRequestPacket : Packet
    {
        public C2SShopDataRequestPacket(Packet packet)
            : base(packet)
        {
            this.Category = packet.ReadByte();
            this.Part = packet.ReadByte();
            this.Character = packet.ReadByte();
            this.Page = packet.ReadByte();
        }

        public byte Category;
        public byte Part;
        public byte Character;
        public byte Page;
    }
}
