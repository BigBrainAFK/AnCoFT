using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildDataPacket : Packet
    {
        public S2CGuildDataPacket()
            : base(Networking.Packet.PacketId.S2CGuildDataAnswer)
        {
            this.Write((short)0);

            this.Write(0);
            this.Write(0); // Guild Icon
            this.Write(0);
            this.Write(0);
            this.Write(0);
            this.Write(0);
            this.Write(0);

            this.Write("GuildName");
            this.Write("Master");
            this.Write((byte) 2); // Sub Master Count
            this.Write("Sub-Master");
            this.Write("Sub-Master2");

            this.Write((byte)1); // Member Count
            this.Write((byte)2); // Max Member Count
            this.Write((byte)3); // Reserve Club Member
            this.Write((byte)4); // Level

            this.Write(1); // Club Point
            this.Write(2); // League Point
            this.Write(3); // Club Gold
            this.Write(4); // Battle Record Win
            this.Write(5); // Battle Record Loose
            this.Write(6); // League Record Win
            this.Write(7); // League Record Loose

            this.Write((byte)4); // Level Restriction

            this.Write((byte)0);

            this.Write((byte)1); // 0 = Private , 1 = Public

            this.Write("Introduction");
            this.Write("Notice");

            this.Write(0);
            this.Write(26050460); // Creation Date Unknown Format
            this.Write((byte)0);
        }

        public S2CGuildDataPacket(short result, Database.Models.Guild guild = null)
            : base(Networking.Packet.PacketId.S2CGuildDataAnswer)
        {
            if (guild == null || guild.ID <= 0 || result == 1)
            {
                this.Write((short) 1);
                return;
            }
            else if (result == -1)
            {
                this.Write((short)-1);
                return;
            }
            this.Write((short)0);
            this.Write(guild.ID);
            this.Write(guild.Emblem);
            this.Write(0);
            this.Write(0);
            this.Write(0);
            this.Write(0);
            this.Write(0);

            this.Write(guild.Name);

            GuildMember masterMember = guild.Members.FirstOrDefault(gm => gm.Division == 3);
            this.Write(masterMember?.Character.Name);

            List<GuildMember> subMasterMembers = guild.Members.Where(gm => gm.Division == 2).ToList();
            this.Write((byte)subMasterMembers.Count); // Sub Master Count

            foreach (GuildMember subMaster in subMasterMembers)
            {
                this.Write(subMaster.Character.Name);
            }
            this.Write((byte)guild.Members.Count); // Member Count
            this.Write(guild.MaxMemberCount); // Max Member Count

            List<GuildMember> reverseMembers = guild.Members.Where(gm => gm.WaitingForApproval == true).ToList();
            this.Write((byte)reverseMembers.Count); // Reserve Club Member

            this.Write(guild.Level); // Level
            this.Write(guild.ClubPoints); // Club Point
            this.Write(guild.LeaguePoints); // League Point
            this.Write(guild.ClubGold); // Club Gold
            this.Write(guild.BattleRecordWin); // Battle Record Win
            this.Write(guild.BattleRecordLoose); // Battle Record Loose
            this.Write(guild.LeagueRecordWin); // League Record Win
            this.Write(guild.LeagueRecordLoose); // League Record Loose
            this.Write(guild.LevelRestriction); // Level Restriction

            this.Write((byte)0);
            // Allowed Character Types

            this.Write(guild.Public); // 0 = Private , 1 = Public
            this.Write(guild.Introduction);
            this.Write(guild.Notice);

            this.Write(0);

            this.Write(guild.CreationDay); // Creation Date Unknown Format

            this.Write((byte)0);
        }
    }
}
