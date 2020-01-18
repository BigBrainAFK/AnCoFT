using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildListAnswerPacket : Packet
    {
        public S2CGuildListAnswerPacket(List<Database.Models.Guild> guildList)
            : base(Networking.Packet.PacketId.S2CGuildListAnswer)
        {
            this.Write((byte) guildList.Count);
            for (int i = 0; i < guildList.Count; i++)
            {
                this.Write(i+1); // Num
                this.Write(guildList[i].ID); // ID
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(guildList[i].Name);
                this.Write(guildList[i].Public); // Public
                this.Write(guildList[i].Members.First(gm => gm.Division == 3).Character.Name);
                this.Write(guildList[i].Level); // Level
                this.Write(guildList[i].LevelRestriction); // Level Restriction
                this.Write((byte)guildList[i].Members.Count); // Member Count
                this.Write(guildList[i].MaxMemberCount); // Max Member Count
                this.Write(guildList[i].BattleRecordWin); // Battle Record Win
                this.Write(guildList[i].BattleRecordLoose); // Battle Record Loose
                this.Write(guildList[i].LeagueRecordWin); // League Record Win
                this.Write(guildList[i].LeagueRecordLoose); // League Record Loose
                this.Write(guildList[i].Introduction);
                this.Write(1); // Unknown
                this.Write(1); // Unknown
                this.Write((byte) 0); // Unknown
                this.Write((byte)guildList[i].AllowedCharacterType.Count); // Allowed Character Count

                foreach (byte allowedCharacter in guildList[i].AllowedCharacterType)
                    this.Write(allowedCharacter);
            }
        }
    }
}
