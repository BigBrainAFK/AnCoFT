namespace AnCoFT.Game.SinglePlay.Challenge
{
    using System;

    public abstract class ChallengeGame
    {
        public short ChallengeId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}