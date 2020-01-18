using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby
{
    public class S2CLobbyUserListAnswerPacket : Packet
    {
        public S2CLobbyUserListAnswerPacket(List<Database.Models.Character> characterList)
            : base(Networking.Packet.PacketId.S2CLobbyUserListAnswer)
        {
            this.Write((byte)characterList.Count);
            for (int i = 0; i < characterList.Count; i++)
            {
                this.Write((short)i);
                this.Write(characterList[i].Name);
                this.Write(characterList[i].CharacterId);
                this.Write(characterList[i].Type);
            }
        }
    }
}
