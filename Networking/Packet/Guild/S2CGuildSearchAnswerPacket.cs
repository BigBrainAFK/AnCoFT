namespace AnCoFT.Networking.Packet.Guild
{
    using System.Collections.Generic;
    using System.Linq;
    using AnCoFT.Game.Guild;

    public class S2CGuildSearchAnswerPacket : Packet
    {
        public S2CGuildSearchAnswerPacket(List<Database.Models.Guild> guildList)
            : base(Networking.Packet.PacketId.S2CGuildSearchAnswer)
        {
            this.Write((byte)guildList.Count);
            for (int i = 0; i < guildList.Count; i++)
            {
                this.Write(i + 1);
                this.Write(guildList[i].Id);
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(0); // Unknown
                this.Write(guildList[i].Name);
                this.Write(guildList[i].Public);
                this.Write(guildList[i].Members.FirstOrDefault(gm => gm.Division == GuildDivision.ClubMaster).Character.Name);
                this.Write(guildList[i].Level);
                this.Write(guildList[i].LevelRestriction);
                this.Write((byte)guildList[i].Members.Count);
                this.Write(guildList[i].MaxMemberCount);
                this.Write(guildList[i].BattleRecordWin);
                this.Write(guildList[i].BattleRecordLoose);
                this.Write(guildList[i].LeagueRecordWin);
                this.Write(guildList[i].LeagueRecordLoose);
                this.Write(guildList[i].Introduction);
                this.Write(guildList[i].CreationDay);
                this.Write((byte)guildList[i].AllowedCharacterType.Length);
                foreach (byte allowedCharacter in guildList[i].AllowedCharacterType)
                    this.Write(allowedCharacter);
            }
        }
    }
}
