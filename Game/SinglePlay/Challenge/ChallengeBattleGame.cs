using System;

namespace AnCoFT.Game.SinglePlay.Challenge
{
    public class ChallengeBattleGame : ChallengeGame
    {
        public ChallengeBattleGame(short challengeId)
        {
            this.ChallengeId = challengeId;
            this.PlayerHp = 0;
            this.NpcHp = 0;
            this.StartTime = DateTime.Now;
        }

        public int PlayerHp { get; set; }

        public int NpcHp { get; set; }

        public bool Finished { get; set; }

        public void SetHp(byte player, int hp)
        {
            if (player == 1)
                this.PlayerHp += hp;
            else
                this.NpcHp += hp;

            if (this.PlayerHp <= 0 || this.NpcHp <= 0)
            {
                this.Finished = true;
                this.EndTime = DateTime.Now;
            }
        }
    }
}