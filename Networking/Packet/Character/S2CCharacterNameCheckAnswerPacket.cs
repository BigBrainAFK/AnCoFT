using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class S2CCharacterNameCheckAnswerPacket : Packet
    {
        public S2CCharacterNameCheckAnswerPacket(short result)
            : base(Networking.Packet.PacketId.S2CCharacterNameCheckAnswer)
        {
            this.Write(result);
        }
    }
}
