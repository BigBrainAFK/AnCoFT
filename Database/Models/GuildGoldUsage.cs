using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class GuildGoldUsage
    {
        public int Id { get; set; }
        public int Date { get; set; }
        public int GoldUsed { get; set; }
        public int GoldRemaining { get; set; }
        public Character Character { get; set; }
    }
}
