using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Shop
{
    public class S2CShopDataAnswerPacket : Packet
    {
        public S2CShopDataAnswerPacket()
            : base(Networking.Packet.PacketId.S2CShopAnswerData)
        {
            this.Write(0);
        }
    }
}
