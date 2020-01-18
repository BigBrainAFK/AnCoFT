using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Game.Chat;

namespace AnCoFT.Networking.Packet.Lobby
{
    public class S2CLobbyChatAnswerPacket : Packet
    {
        public S2CLobbyChatAnswerPacket(ChatType type, string characterName, string message)
            : base(Networking.Packet.PacketId.S2CLobbyChatAnswer)
        {
            this.Write(type);
            this.Write(characterName);
            this.Write(message);
        }
    }
}
