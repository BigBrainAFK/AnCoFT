namespace AnCoFT.Database.Models
{
    public abstract class ChallengeReward
    {
        public bool ItemRewardRepeat { get; set; }

        public int QuantityMax1 { get; set; }

        public int QuantityMax2 { get; set; }

        public int QuantityMax3 { get; set; }

        public int QuantityMin1 { get; set; }

        public int QuantityMin2 { get; set; }

        public int QuantityMin3 { get; set; }

        public byte RewardExp { get; set; }

        public int RewardGold { get; set; }

        public int RewardItem1 { get; set; }

        public int RewardItem2 { get; set; }

        public int RewardItem3 { get; set; }
    }
}
