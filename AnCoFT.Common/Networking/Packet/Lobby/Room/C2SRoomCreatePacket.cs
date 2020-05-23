using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Lobby.Room
{
    public class C2SRoomCreatePacket : Packet
    {
        public C2SRoomCreatePacket(Packet packet)
            : base(packet)
        {
            this.Name = this.ReadUnicodeString();
            this.Type = this.ReadByte();
            this.GameMode = this.ReadByte();
            this.Rule = this.ReadByte();
            this.Players = this.ReadByte();
            this.Private = Convert.ToBoolean(this.ReadByte());
            this.Unknown = this.ReadByte();
            this.SkillFree = Convert.ToBoolean(this.ReadByte());
            this.QuickSlot = Convert.ToBoolean(this.ReadByte());
            this.LevelRange = this.ReadByte();
            this.BettingType = this.ReadShort();
            this.BettingAmount = this.ReadInteger();
            this.Ball = this.ReadInteger();

            if (this.Private)
                this.Password = this.ReadUnicodeString();
        }

        public string Name { get; set; }
        public byte Type { get; set; }
        public byte GameMode { get; set; }
        public byte Rule { get; set; }
        public byte Players { get; set; }
        public bool Private { get; set; }
        public byte Unknown { get; set; }
        public bool SkillFree { get; set; }
        public bool QuickSlot { get; set; }
        public byte LevelRange { get; set; }
        public short BettingType { get; set; }
        public int BettingAmount { get; set; }
        public int Ball { get; set; }
        public string Password { get; set; }
    }
}
