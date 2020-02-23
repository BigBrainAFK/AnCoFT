namespace AnCoFT.Database.Models
{
    using System;

    public class GuildGoldUsage
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int GoldUsed { get; set; }

        public int GoldRemaining { get; set; }

        public Character Character { get; set; }
    }
}
