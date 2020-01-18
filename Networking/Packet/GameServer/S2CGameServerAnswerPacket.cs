using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.GameServer
{
    public class S2CGameServerAnswerPacket : Packet
    {
        public S2CGameServerAnswerPacket(byte requestType)
            : base(Networking.Packet.PacketId.S2CGameAnswerData)
        {
            this.Write(requestType);
            this.Write((byte)0);
        }
    }
}
