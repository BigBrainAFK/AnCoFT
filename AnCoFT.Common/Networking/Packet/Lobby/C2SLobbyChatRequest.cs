using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Game.Chat;

namespace AnCoFT.Networking.Packet.Lobby
{
    public class C2SLobbyChatRequestPacket : Packet
    {
        public C2SLobbyChatRequestPacket(Packet packet)
            : base(packet)
        {
            this.Type = (ChatType)this.ReadShort();
            this.Message = this.ReadUnicodeString();
        }

        public ChatType Type { get; }
        public string Message { get; }
    }
}
