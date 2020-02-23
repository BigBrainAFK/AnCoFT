namespace AnCoFT.Networking.Packet.Guild
{
    using System.Collections.Generic;
    using System.Linq;
    using AnCoFT.Database.Models;

    public class S2CGuildMemberDataPacket : Packet
    {
        public S2CGuildMemberDataPacket(List<GuildMember> memberList)
            : base(Networking.Packet.PacketId.S2CGuildMemberDataAnswer)
        {
            this.Write((short)0);
            this.Write((byte)memberList.Count);

            for (int i = 0; i < memberList.Count; i++)
            {
                this.Write(i + 1);
                this.Write(memberList[i].CharacterId);
                this.Write(memberList[i].Division);
                this.Write(memberList[i].Character.Level);
                this.Write(memberList[i].Character.Type);
                this.Write(memberList[i].Character.Name);
                this.Write(memberList[i].ContributionPoints);
                this.Write((short)0); // Unknown
            }
        }

        public S2CGuildMemberDataPacket(Guild guild)
            : base(Networking.Packet.PacketId.S2CGuildMemberDataAnswer)
        {
            this.Write((short)0);

            List<GuildMember> guildMembers = guild.Members.Where(gm => gm.WaitingForApproval == false).ToList();
            this.Write((byte)guildMembers.Count);

            for (int i = 1; i <= guildMembers.Count; i++)
            {
                this.Write(i);
                this.Write(guildMembers[i].Character.CharacterId);
                this.Write(guildMembers[i].Division);
                this.Write(guildMembers[i].Character.Level);
                this.Write(guildMembers[i].Character.Type);
                this.Write(guildMembers[i].Character.Name);
                this.Write(guildMembers[i].ContributionPoints);
            }
        }
    }
}
