using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class S2CRoomMapChangeAnswerPacket : Packet
    {
        public S2CRoomMapChangeAnswerPacket(byte map)
            : base(Networking.Packet.PacketId.S2CRoomMapChangeAnswer)
        {
            this.Write(map);
        }
    }
}
