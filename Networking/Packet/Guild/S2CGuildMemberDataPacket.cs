using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildMemberDataPacket : Packet
    {
        public S2CGuildMemberDataPacket(List<GuildMember> memberList)
            : base(Networking.Packet.PacketId.S2CGuildMemberDataAnswer)
        {
            this.Write((short)0);
            this.Write((byte)memberList.Count); // Member Count

            for (int i = 0; i < memberList.Count; i++)
            {
                this.Write(i+1); // Num or AccId
                this.Write(memberList[i].CharacterId); // CharacterID
                this.Write(memberList[i].Division); // Division 1 = Club Member, 2 = Sub Club Master, 3 = Club Master
                this.Write(memberList[i].Character.Level); // Level
                this.Write(memberList[i].Character.Type); // CharacterType
                this.Write(memberList[i].Character.Name); // Character Name
                this.Write(memberList[i].ContributionPoints); // Contribution Points
                this.Write((short) 0); // Unknown
            }
        }

        public S2CGuildMemberDataPacket(Database.Models.Guild guild)
            : base(Networking.Packet.PacketId.S2CGuildMemberDataAnswer)
        {
            this.Write((short)0);

            List<GuildMember> guildMembers = guild.Members.Where(gm => gm.WaitingForApproval == false).ToList();
            this.Write((byte)guildMembers.Count); // Member Count

            for (int i = 1; i <= guildMembers.Count; i++)
            {
                this.Write(i); // Num or AccId?
                this.Write(guildMembers[i].Character.CharacterId); // CharacterID

                this.Write((byte)guildMembers[i].Division); // Division 1 = Club Member, 2 = Sub Club Master, 3 = Club Master
                this.Write(guildMembers[i].Character.Level); // Level
                this.Write(guildMembers[i].Character.Type); // CharacterType
                this.Write(guildMembers[i].Character.Name);
                this.Write(guildMembers[i].ContributionPoints); // Contribution Points
            }
        }
    }
}
