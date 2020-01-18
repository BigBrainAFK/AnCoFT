using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Shop
{
    public class S2CShopDataPrepareAnswerPacket : Packet
    {
        public S2CShopDataPrepareAnswerPacket(byte category, byte part, byte character, int count)
            : base(Networking.Packet.PacketId.S2CShopAnswerDataPrepare)
        {
            this.Write(category);
            this.Write(part);
            this.Write(character);
            this.Write(count);
        }
    }
}
