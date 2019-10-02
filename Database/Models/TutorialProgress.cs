namespace AnCoFT.Database.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class TutorialProgress
    {
        public int CharacterId { get; set; }

        public short TutorialId { get; set; }

        public short Success { get; set; }

        public short Attempts { get; set; }

        [ForeignKey("TutorialId")]
        public Tutorial Tutorial { get; set; }
    }
}
