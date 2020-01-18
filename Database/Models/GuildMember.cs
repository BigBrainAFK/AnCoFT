using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class GuildMember
    {
        public int CharacterId { get; set; }
        public byte Division { get; set; }
        public int ContributionPoints { get; set; }
        public int RequestDate { get; set; }
        public bool WaitingForApproval { get; set; }

        [ForeignKey("CharacterId")]
        public Character Character { get; set; }
        public int GuildId { get; set; }
        [ForeignKey("GuildId")]
        public Guild Guild { get; set; }
    }
}
