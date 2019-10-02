namespace AnCoFT.Database.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChallengeProgress
    {
        public int CharacterId { get; set; }

        public short ChallengeId { get; set; }

        public short Success { get; set; }

        public byte Attempts { get; set; }

        [ForeignKey("ChallengeId")]
        public Challenge Challenge { get; set; }
    }
}