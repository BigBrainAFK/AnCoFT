using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    using AnCoFT.Database.Models;

    public class S2CCharacterLevelExpData : Packet
    {
        public S2CCharacterLevelExpData(Character character)
            : base(Networking.Packet.PacketId.S2CCharacterLevelExpData)
        {
            this.Write(character.Level);
            this.Write(character.Exp);
        }
    }
}
