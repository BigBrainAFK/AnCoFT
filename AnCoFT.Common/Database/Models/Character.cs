using System.ComponentModel.DataAnnotations.Schema;

namespace AnCoFT.Database.Models
{
    using System.Collections.Generic;

    public class Character
    {
        public Character()
        {
            this.Name = "";
            this.Level = 1;
            this.Type = 1;
            this.AlreadyCreated = false;
            this.BattlesLost = 0;
            this.BattlesWon = 0;
            this.Dexterity = 0;
            this.Willpower = 0;
            this.Stamina = 0;
            this.Strength = 0;
            this.StatusPoints = 0;
            this.Gold = 0;
            this.Hair = 0;
            this.Face = 0;
            this.Dye = 0;
            this.Dress = 0;
            this.Glasses = 0;
            this.Gloves = 0;
            this.Bag = 0;
            this.Hat = 0;
            this.Shoes = 0;
            this.Pants = 0;
            this.Socks = 0;
            this.Racket = 0;
            this.NameChangeAllowed = false;
            this.Exp = 0;
        }
        public int MaxInventoryCount { get; set; }
        public int Exp { get; set; }
        public bool AlreadyCreated { get; set; }

        public int Bag { get; set; }
         
        public int BattlesLost { get; set; }

        public int BattlesWon { get; set; }

        public List<ChallengeProgress> ChallengeProgress { get; set; }
        public List<TutorialProgress> TutorialProgress { get; set; }

        public int CharacterId { get; set; }

        public byte Dexterity { get; set; }

        public int Dress { get; set; }

        public int Dye { get; set; }

        public int Face { get; set; }

        public int Glasses { get; set; }

        public int Gloves { get; set; }

        public int Gold { get; set; }

        public int Hair { get; set; }

        public int Hat { get; set; }

        public byte Level { get; set; }

        public string Name { get; set; }

        public bool NameChangeAllowed { get; set; }

        public int Pants { get; set; }
        
        public int Racket { get; set; }

        public int Shoes { get; set; }

        public int Socks { get; set; }

        public byte Stamina { get; set; }

        public byte StatusPoints { get; set; }

        public byte Strength { get; set; }

        public byte Type { get; set; }

        public byte Willpower { get; set; }
        public GuildMember GuildMember { get; set; }
        public List<CharacterInventory> Inventory { get; set; }
        public List<MessengerFriend> MessengerFriendList { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
    }
}
