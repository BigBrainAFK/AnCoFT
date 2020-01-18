using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class S2CCharacterDeleteAnswerPacket : Packet
    {
        public S2CCharacterDeleteAnswerPacket(short result)

            : base(Networking.Packet.PacketId.S2CCharacterDelete)
        {
            this.Write(result);
        }
    }
}
