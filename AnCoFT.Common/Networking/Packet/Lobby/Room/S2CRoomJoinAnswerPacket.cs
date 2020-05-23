using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomJoinAnswerPacket : Packet
    {
        public S2CRoomJoinAnswerPacket(short result, byte roomType, byte unknown, byte unknown2)
            : base(Networking.Packet.PacketId.S2CRoomJoinAnswer)
        {
            this.Write(result);
            this.Write(roomType);
            this.Write(unknown);
            this.Write(unknown2);
        }
    }
}
