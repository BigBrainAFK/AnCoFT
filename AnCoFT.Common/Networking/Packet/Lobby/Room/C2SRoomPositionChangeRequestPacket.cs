using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class C2SRoomPositionChangeRequestPacket : Packet
    {
        public C2SRoomPositionChangeRequestPacket(Packet packet)
            : base(packet)
        {
            this.NewPosition = this.ReadShort();
        }

        public short NewPosition { get; set; }
    }
}
