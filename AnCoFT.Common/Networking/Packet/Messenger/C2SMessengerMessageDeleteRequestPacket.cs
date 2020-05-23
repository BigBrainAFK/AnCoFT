using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Messenger
{
    public class C2SMessengerMessageDeleteRequestPacket : Packet
    {
        public C2SMessengerMessageDeleteRequestPacket(Packet packet)
            : base(packet)
        {
            this.Unknown = this.ReadByte();
            this.Unknown2 = this.ReadByte();
            this.MessageId = this.ReadInteger();
        }

        public byte Unknown { get; set; }
        public byte Unknown2 { get; set; }
        public int MessageId { get; set; }
    }
}
