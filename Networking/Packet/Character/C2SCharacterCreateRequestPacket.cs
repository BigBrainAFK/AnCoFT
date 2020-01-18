using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Character
{
    public class C2SCharacterCreateRequestPacket : Packet
    {
        public C2SCharacterCreateRequestPacket(Packet packet)
            : base(packet)
        {
            this.CharacterId = this.ReadInteger();
            this.Nickname = this.ReadUnicodeString();
            this.Strength = this.ReadByte();
            this.Stamina = this.ReadByte();
            this.Dexterity = this.ReadByte();
            this.Willpower = this.ReadByte();
            this.StatusPoints = this.ReadByte();
            this.CharacterType = this.ReadByte();
        }

        public int CharacterId { get; }
        public string Nickname { get; }
        public byte Strength { get; }
        public byte Stamina { get; }
        public byte Dexterity { get; }
        public byte Willpower { get; }
        public byte StatusPoints { get; }
        public byte CharacterType { get; }
    }
}
