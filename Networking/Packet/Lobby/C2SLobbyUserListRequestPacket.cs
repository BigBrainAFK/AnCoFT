using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby
{
    public class C2SLobbyUserListRequestPacket : Packet
    {
        public C2SLobbyUserListRequestPacket(Packet packet)
            : base(packet)
        {
            this.Page = this.ReadByte();
        }

        public byte Page { get; set; }
    }
}
