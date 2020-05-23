using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildLeagueRankingAnswerPacket : Packet
    {
        public S2CGuildLeagueRankingAnswerPacket(List<Database.Models.Guild> guildList)
            : base(Networking.Packet.PacketId.S2CGuildLeagueRankingAnswer)
        {
            this.Write((byte)0);
            this.Write((short)guildList.Count);

            foreach (Database.Models.Guild guild in guildList)
            {
                this.Write(guild.Name);
                this.Write(guild.Level); // Level
                this.Write(guild.LeaguePoints); // League Points
                this.Write(guild.LeagueRecordWin); // Win
                this.Write(guild.LeagueRecordLoose); // Loose
                this.Write(0); // Unknown Emblem
                this.Write(0); // Unknown
                this.Write(0); // Unknown Emblem
                this.Write(0); // Unknown
                this.Write(0); // Unknown Emblem
                this.Write(0); // Unknown
            }
        }
    }
}
