using System;
using AnCoFT.Game.Messenger;

namespace AnCoFT.Database.Models
{
    public class MessengerMessage
    {
        public int Id { get; set; }
        public Character From { get; set; }
        public Character To { get; set; }
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public int ItemId { get; set; }
        public byte ItemQty { get; set; }
    }
}
