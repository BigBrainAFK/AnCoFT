using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class S2CWelcomePacket : Packet
    {
        public S2CWelcomePacket(int decKey, int encKey, int decTblIdx, int encTblIdx)
            : base(Networking.Packet.PacketId.S2CLoginWelcomePacket)
        {
            this.Write(decKey);
            this.Write(encKey);
            this.Write(decTblIdx);
            this.Write(encTblIdx);
        }
    }
}
