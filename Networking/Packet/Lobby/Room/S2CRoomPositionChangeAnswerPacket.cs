using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomPositionChangeAnswerPacket : Packet
    {
        public S2CRoomPositionChangeAnswerPacket(short result, short oldPosition, short newPosition)
            : base(Networking.Packet.PacketId.S2CRoomPositionChangeAnswer)
        {
            this.Write(result);
            this.Write(oldPosition);
            this.Write(newPosition);
        }
    }
}
