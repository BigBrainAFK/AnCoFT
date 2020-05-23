namespace AnCoFT.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using AnCoFT.Game.Guild;

    public class GuildMember
    {
        public int CharacterId { get; set; }

        public GuildDivision Division { get; set; }

        public int ContributionPoints { get; set; }

        public DateTime RequestDate { get; set; }

        public bool WaitingForApproval { get; set; }

        [ForeignKey("CharacterId")]
        public Character Character { get; set; }

        public int GuildId { get; set; }

        [ForeignKey("GuildId")]
        public Guild Guild { get; set; }
    }
}
