namespace AnCoFT.Game.MatchPlay.Room
{
    using System.Collections.Generic;

    public class Room
    {
        public Room()
        {
            this.CurrentPlayers = new List<RoomPlayer>();
        }

        public short Id { get; set; }

        public string Name { get; set; }

        public byte GameMode { get; set; }

        public byte BattleMode { get; set; }

        public bool Betting { get; set; }

        public byte BettingMode { get; set; }

        public byte BettingCoins { get; set; }

        public int BettingGold { get; set; }

        public byte Ball { get; set; }

        public byte MaxPlayer { get; set; }

        public bool Private { get; set; }

        public byte Level { get; set; }

        public byte LevelRange { get; set; }

        public byte Map { get; set; }

        public List<RoomPlayer> CurrentPlayers { get; set; }
    }
}
