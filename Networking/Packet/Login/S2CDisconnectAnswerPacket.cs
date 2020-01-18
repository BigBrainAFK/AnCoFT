using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CDisconnectAnswerPacket : Packet
    {
        public S2CDisconnectAnswerPacket()
            : base(Networking.Packet.PacketId.S2CDisconnectAnswer)
        {
            this.Write((byte)0);
        }
    }
}
