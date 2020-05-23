using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class C2SMessengerMessageSendRequestPacket : Packet
    {
        public C2SMessengerMessageSendRequestPacket(Packet packet)
            : base(packet)
        {
            this.FriendCharacterName = this.ReadUnicodeString();
            this.Message = this.ReadUnicodeString();
        }

        public string FriendCharacterName { get; set; }
        public string Message { get; set; }
    }
}
