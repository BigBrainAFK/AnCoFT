namespace AnCoFT.Game.MatchPlay.Room
{
    using AnCoFT.Database.Models;

    public class RoomPlayer
    {
        public Character Character { get; set; }

        public byte Position { get; set; }

        public bool Master { get; set; }

        public bool Ready { get; set; }

        public bool Fitting { get; set; }
    }
}