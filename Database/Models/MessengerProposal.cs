using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class MessengerProposal
    {
        public int Id { get; set; }
        public Character From { get; set; }
        public Character To { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }
    }
}
