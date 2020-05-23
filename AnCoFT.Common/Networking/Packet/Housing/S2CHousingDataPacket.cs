using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Networking.Packet.Home
{
    using AnCoFT.Database.Models;

    public class S2CHousingDataPacket : Packet
    {
        public S2CHousingDataPacket(Home home)
            : base(Networking.Packet.PacketId.S2CHomeData)
        {
            this.Write(home.Level);
            this.Write(home.HousingPoints);
            this.Write(home.FamousPoints);
            this.Write(home.FurnitureCount);
            this.Write(home.BasicBonusExp);
            this.Write(home.BasicBonusGold);
            this.Write(home.BattleBonusExp);
            this.Write(home.BattleBonusGold);
        }
    }
}
