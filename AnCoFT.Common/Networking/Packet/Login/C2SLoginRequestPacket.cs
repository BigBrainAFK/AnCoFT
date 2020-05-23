using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Login
{
    public class C2SLoginRequestPacket : Packet
    {
        public C2SLoginRequestPacket(Packet packet)
            : base(packet)
        {
            this.Username = this.ReadUnicodeString();
            this.Password = this.ReadString();
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
