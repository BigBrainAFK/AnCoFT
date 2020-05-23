namespace AnCoFT.Networking.Packet.Guild
{
    using System.Collections.Generic;
    using AnCoFT.Database.Models;

    public class S2CGuildReserveMemberDataPacket : Packet
    {
        public S2CGuildReserveMemberDataPacket(List<GuildMember> guildMembers)
            : base(Networking.Packet.PacketId.S2CGuildReserveMemberDataAnswer)
        {
            this.Write((short)0);
            this.Write((byte)guildMembers.Count);

            for (int i = 0; i < guildMembers.Count; i++)
            {
                this.Write(guildMembers[i].Character.CharacterId);
                this.Write(guildMembers[i].Character.Level);
                this.Write(guildMembers[i].Character.Type);
                this.Write(guildMembers[i].Character.Name);
                this.Write(guildMembers[i].RequestDate);
            }
        }
    }
}
