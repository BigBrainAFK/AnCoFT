using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class C2SMessengerFriendAddRequestPacket : Packet
    {
        public C2SMessengerFriendAddRequestPacket(Packet packet)
            : base(packet)
        {
            this.FriendCharacterName = this.ReadUnicodeString();
        }

        public string FriendCharacterName { get; set; }
    }
}
