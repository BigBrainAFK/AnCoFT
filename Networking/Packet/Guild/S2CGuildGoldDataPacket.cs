using System;
using System.Collections.Generic;
using System.Text;
using AnCoFT.Database.Models;

namespace AnCoFT.Networking.Packet.Guild
{
    public class S2CGuildGoldDataPacket : Packet
    {
        public S2CGuildGoldDataPacket(Database.Models.Guild guild)
            : base(Networking.Packet.PacketId.S2CGuildGoldDataAnswer)
        {
            this.Write((short)0);

            if (guild.GoldUsages?.Count == null || guild.GoldUsages?.Count == 0)
            {
                this.Write((byte) 0);
                return;
            }

            for (int i = 0; i < guild.GoldUsages.Count; i++)
            {
                this.Write(i);
                this.Write(guild.GoldUsages[i].Character.CharacterId);
                this.Write(guild.GoldUsages[i].Date);
                this.Write(guild.GoldUsages[i].GoldUsed);
                this.Write(guild.GoldUsages[i].Character.Name);
                this.Write(guild.GoldUsages[i].GoldRemaining);
            }
        }
    }
}
