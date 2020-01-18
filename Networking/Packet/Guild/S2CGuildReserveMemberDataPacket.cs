using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildReserveMemberDataPacket : Packet
    {
        public S2CGuildReserveMemberDataPacket()
            : base(Networking.Packet.PacketId.S2CGuildReserveMemberDataAnswer)
        {
            this.Write((short)0);

            this.Write((byte)1); // Reserve Member Count
            this.Write(1); // CharacterID
            this.Write((byte)2); // Level
            this.Write((byte)3); // Type
            this.Write("Name");
            this.Write(40); // Request Day
        }

        public S2CGuildReserveMemberDataPacket(Database.Models.Guild guild)
            : base(Networking.Packet.PacketId.S2CGuildReserveMemberDataAnswer)
        {
            this.Write((short)0);

            List<GuildMember> guildMembers = guild.Members.Where(gm => gm.WaitingForApproval == true).ToList();
            this.Write((byte)guildMembers.Count); // Member Count

            for (int i = 1; i <= guildMembers.Count; i++)
            {
                this.Write(guildMembers[i].Character.CharacterId); // CharacterID
                this.Write(guildMembers[i].Character.Level); // Level
                this.Write(guildMembers[i].Character.Type); // Type
                this.Write(guildMembers[i].Character.Name);
                this.Write(guildMembers[i].RequestDate); // Request Day
            }
        }
    }
}
