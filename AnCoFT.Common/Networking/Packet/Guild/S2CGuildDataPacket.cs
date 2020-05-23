namespace AnCoFT.Networking.Packet.Guild
{
    using System.Collections.Generic;
    using System.Linq;
    using AnCoFT.Database.Models;
    using AnCoFT.Game.Guild;

    public class S2CGuildDataPacket : Packet
    {
        public S2CGuildDataPacket(GuildStatus status, Guild guild = null)
            : base(Networking.Packet.PacketId.S2CGuildDataAnswer)
        {
            if (guild == null || guild.Id <= 0 || status == GuildStatus.NoGuild)
            {
                this.Write(status);
                return;
            }
            else if (status == GuildStatus.WaitingForApproval)
            {
                this.Write(status);
                return;
            }

            this.Write((short)0);
            this.Write(guild.Id);
            this.Write(guild.Emblem);
            this.Write(0); // Unknown
            this.Write(0); // Unknown
            this.Write(0); // Unknown
            this.Write(0); // Unknown
            this.Write(0); // Unknown

            this.Write(guild.Name);

            GuildMember masterMember = guild.Members.FirstOrDefault(gm => gm.Division == GuildDivision.ClubMaster);
            this.Write(masterMember?.Character.Name);

            List<GuildMember> subMasterMembers = guild.Members.Where(gm => gm.Division == GuildDivision.SubClubMaster).ToList();
            this.Write((byte)subMasterMembers.Count);

            foreach (GuildMember subMaster in subMasterMembers)
                this.Write(subMaster.Character.Name);

            this.Write((byte)guild.Members.Count);
            this.Write(guild.MaxMemberCount);

            List<GuildMember> reverseMembers = guild.Members.Where(gm => gm.WaitingForApproval == true).ToList();
            this.Write((byte)reverseMembers.Count);

            this.Write(guild.Level);
            this.Write(guild.ClubPoints);
            this.Write(guild.LeaguePoints);
            this.Write(guild.ClubGold);
            this.Write(guild.BattleRecordWin);
            this.Write(guild.BattleRecordLoose);
            this.Write(guild.LeagueRecordWin);
            this.Write(guild.LeagueRecordLoose);
            this.Write(guild.LevelRestriction);

            this.Write((byte)guild.AllowedCharacterType.Length);
            foreach (byte allowedType in guild.AllowedCharacterType)
                this.Write(allowedType);

            this.Write(guild.Public);
            this.Write(guild.Introduction);
            this.Write(guild.Notice);
            this.Write(guild.CreationDay);
            this.Write((byte)0); // Unknown
        }
    }
}
