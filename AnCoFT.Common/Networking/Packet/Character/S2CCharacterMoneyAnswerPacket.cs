using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class S2CCharacterMoneyAnswerPacket : Packet
    {
        public S2CCharacterMoneyAnswerPacket(int ap, int gold)
            : base(Networking.Packet.PacketId.S2CCharacterMoneyAnswer)
        {
            this.Write(ap);
            this.Write(gold);
        }
    }
}
