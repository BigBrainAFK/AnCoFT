using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CLoginFirstCharacterAnswerPacket : Packet
    {
        public S2CLoginFirstCharacterAnswerPacket(short result, int characterId = 0, byte characterType = 0)
            : base(Networking.Packet.PacketId.S2CLoginFirstCharacterAnswer)
        {
            this.Write(result);
            this.Write(characterId);
            this.Write(characterType);
        }
    }
}
