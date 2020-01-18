using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class Home
    {
        public Home()
        {
            this.Level = 0;
            this.HousingPoints = 0;
            this.FamousPoints = 0;
            this.FurnitureCount = 0;
            this.BasicBonusExp = 0;
            this.BasicBonusGold = 0;
            this.BattleBonusExp = 0;
            this.BattleBonusGold = 0;
        }

        public int ID { get; set; }
        public byte Level { get; set; }
        public int HousingPoints { get; set; }
        public int FamousPoints { get; set; }
        public int FurnitureCount { get; set; }
        public byte BasicBonusExp { get; set; }
        public byte BasicBonusGold { get; set; }
        public byte BattleBonusExp { get; set; }
        public byte BattleBonusGold { get; set; }
        public List<HomeInventory> Inventory { get; set; }
    }
}
