using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnCoFT.Dashboard.Models
{
	public class CharacterEdit
	{
		[Required]
		[Remote("CharacterIdChanged", "Account", ErrorMessage = "The CharacterId has been tempered with.", AdditionalFields = nameof(Hash))]
		public int CharacterId { get; set; }

		public byte[] Hash { get; set; }

		[Display(Name = "Charactername")]
		[Required(ErrorMessage = "Charactername is required.")]
		[StringLength(14, MinimumLength = 6, ErrorMessage = "Charactername has to be between 6 and 14 characters.")]
		[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Charactername can only consist of alphanumeric characters and \"_\".")]
		[Remote("IsCharacterNameUnique", "Account", ErrorMessage = "Charactername already taken.", AdditionalFields = nameof(CharacterId))]
		public string Name { get; set; }

		[Display(Name = "Name change allowed?")]
		public bool NameChangeAllowed { get; set; }

		[Display(Name = "Level")]
		[Required(ErrorMessage = "Character level has to be specified.")]
		[Range(1, byte.MaxValue, ErrorMessage = "Character level has to be at least 1.")]
		public byte Level { get; set; }

		[Display(Name = "Experience points")]
		[Required(ErrorMessage = "Experience points have to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Experience points have to be a positive number.")]
		public int Exp { get; set; }

		[Display(Name = "Gold")]
		[Required(ErrorMessage = "Gold has to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Gold has to be a positive number.")]
		public int Gold { get; set; }

		[Display(Name = "Max Inventory Space")]
		[Required(ErrorMessage = "Inventory Space has to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Inventory Space has to be a positive number.")]
		public int MaxInventoryCount { get; set; }

		[Display(Name = "Strength")]
		[Required(ErrorMessage = "Strength has to be set.")]
		[Range(0, byte.MaxValue, ErrorMessage = "Strength has to be a positive number.")]
		public byte Strength { get; set; }

		[Display(Name = "Stamina")]
		[Required(ErrorMessage = "Stamina has to be set.")]
		[Range(0, byte.MaxValue, ErrorMessage = "Stamina has to be a positive number.")]
		public byte Stamina { get; set; }

		[Display(Name = "Dexterity")]
		[Required(ErrorMessage = "Dexterity has to be set.")]
		[Range(0, byte.MaxValue, ErrorMessage = "Dexterity has to be a positive number.")]
		public byte Dexterity { get; set; }

		[Display(Name = "Willpower")]
		[Required(ErrorMessage = "Willpower has to be set.")]
		[Range(0, byte.MaxValue, ErrorMessage = "Willpower has to be a positive number.")]
		public byte Willpower { get; set; }

		[Display(Name = "Statuspoints")]
		[Required(ErrorMessage = "Statuspoints has to be set.")]
		[Range(0, byte.MaxValue, ErrorMessage = "Statuspoints has to be a positive number.")]
		public byte StatusPoints { get; set; }

		[Display(Name = "Battles won")]
		[Required(ErrorMessage = "Battles won has to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Battles won has to be a positive number.")]
		public int BattlesWon { get; set; }

		[Display(Name = "Battles lost")]
		[Required(ErrorMessage = "Battles lost has to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Battles lost has to be a positive number.")]
		public int BattlesLost { get; set; }
	}
}
