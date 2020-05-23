using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Game.Login;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CLoginAnswerPacket : Packet
    {
        public S2CLoginAnswerPacket(LoginResult result)
            : base(Networking.Packet.PacketId.S2CLoginAnswerPacket)
        {
            this.Write(result);
        }
    }
}
