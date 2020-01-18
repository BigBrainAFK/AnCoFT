using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnCoFT.Database.Models
{
    public class MessengerFriend
    {
        public int CharacterId { get; set; }
        public int FriendCharacterId { get; set; }
        [ForeignKey("FriendCharacterId")]
        public Character FriendCharacter { get; set; }
    }
}
