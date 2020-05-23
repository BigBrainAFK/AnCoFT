using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class S2CCharacterCreateAnswerPacket : Packet
    {
        public S2CCharacterCreateAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CCharacterCreateAnswer)
        {
            this.Write(result);
        }
    }
}
