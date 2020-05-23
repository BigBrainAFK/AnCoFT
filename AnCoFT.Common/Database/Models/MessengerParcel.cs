using System;

namespace AnCoFT.Database.Models
{
    public class MessengerParcel
    {
        public int Id { get; set; }
        public Character From { get; set; }
        public Character To { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
        public int ItemId { get; set; }
        public byte ItemQty { get; set; }
        public byte SendCurrency { get; set; }
        public int SendAmount { get; set; }
        public byte SellCurrency { get; set; }
        public int SellAmount { get; set; }
    }
}
