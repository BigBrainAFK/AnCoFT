using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class C2SMessengerMessageReadRequestPacket : Packet
    {
        public C2SMessengerMessageReadRequestPacket(Packet packet)
            : base(packet)
        {
            this.Unknown = this.ReadByte();
            this.MessageId = this.ReadInteger();
        }

        public byte Unknown { get; set; }
        public int MessageId { get; set; }
    }
}
