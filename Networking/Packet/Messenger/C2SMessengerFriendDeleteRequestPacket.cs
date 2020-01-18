using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class C2SMessengerFriendDeleteRequestPacket : Packet
    {
        public C2SMessengerFriendDeleteRequestPacket(Packet packet)
            : base(packet)
        {
            this.FriendCharacterId = this.ReadInteger();
        }

        public int FriendCharacterId { get; set; }
    }
}
