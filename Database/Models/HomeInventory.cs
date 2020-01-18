using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class HomeInventory
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public byte Unk0;
        public byte Unk1;
        public byte XPos;
        public byte YPos;
    }
}
