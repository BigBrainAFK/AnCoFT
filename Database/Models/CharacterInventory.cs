using System;
using System.Collections.Generic;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class CharacterInventory
    {
        public int Id { get; set; }
        public byte Category { get; set; }
        public int ItemId { get; set; }
        public byte UseType { get; set; }
        public int ItemQty { get; set; }
    }
}
