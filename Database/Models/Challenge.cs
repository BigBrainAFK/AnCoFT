namespace AnCoFT.Database.Models
{
    using AnCoFT.Game.SinglePlay.Challenge;

    public class Challenge : ChallengeReward
    {
        public short ChallengeId { get; set; }

        public GameType GameType { get; set; }
        public GameMode GameMode { get; set; }

        public byte Level { get; set; }

        public byte LevelRestriction { get; set; }

        public int NpcHp { get; set; }
    }
}