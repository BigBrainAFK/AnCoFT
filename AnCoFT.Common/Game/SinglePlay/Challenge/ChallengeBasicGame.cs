namespace AnCoFT.Game.SinglePlay.Challenge
{
    using System;

    public class ChallengeBasicGame : ChallengeGame
    {
        public ChallengeBasicGame(short challengeId)
        {
            this.ChallengeId = challengeId;
            this.PointsPlayer = 0;
            this.PointsNpc = 0;
            this.SetsPlayer = 0;
            this.SetsNpc = 0;
            this.StartTime = DateTime.Now;
        }

        public byte PointsPlayer { get; set; }

        public byte PointsNpc { get; set; }

        public byte SetsPlayer { get; set; }

        public byte SetsNpc { get; set; }

        public bool Finished { get; set; }

        public void SetPoints(byte pointsPlayer, byte pointsNpc)
        {
            this.PointsPlayer = pointsPlayer;
            this.PointsNpc = pointsNpc;

            if (pointsPlayer == 4 && pointsNpc < 4)
            {
                this.SetsPlayer += 1;
                this.PointsPlayer = 0;
                this.PointsNpc = 0;
            }
            else if (pointsPlayer > 4 && (pointsPlayer - pointsNpc) == 2)
            {
                this.SetsPlayer += 1;
                this.PointsPlayer = 0;
                this.PointsNpc = 0;
            }
            else if (pointsNpc == 4 && pointsPlayer < 4)
            {
                this.SetsNpc += 1;
                this.PointsPlayer = 0;
                this.PointsNpc = 0;
            }
            else if (pointsNpc > 4 && (pointsNpc - pointsPlayer) == 2)
            {
                this.SetsNpc += 1;
                this.PointsPlayer = 0;
                this.PointsNpc = 0;
            }

            if (this.SetsPlayer == 2 || this.SetsNpc == 2)
            {
                this.Finished = true;
                this.EndTime = DateTime.Now;
            }
        }
    }
}