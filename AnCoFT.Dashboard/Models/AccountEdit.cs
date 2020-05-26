using AnCoFT.Database.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AnCoFT.Dashboard.Models
{
	public class AccountEdit
	{
		[Required]
		[Remote("AccountIdChanged", "Account", ErrorMessage = "The AccountId has been tempered with.", AdditionalFields = nameof(Hash))]
		public int AccountId { get; set; }

		public byte[] Hash { get; set; }

		[Display(Name = "Accountname")]
		[Required(ErrorMessage = "Accountname is required.")]
		[StringLength(14, MinimumLength = 6, ErrorMessage = "Accountname has to be between 6 and 14 characters.")]
		[RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Accountname can only consist of alphanumeric characters and \"_\".")]
		[Remote("IsUserUnique", "Account", ErrorMessage = "Accountname already taken.", AdditionalFields = nameof(AccountId))]
		public string Username { get; set; }

		[Display(Name = "Premium")]
		public bool Premium { get; set; }

		[Display(Name = "Ap")]
		[Required(ErrorMessage = "Ap has to be set.")]
		[Range(0, int.MaxValue, ErrorMessage = "Ap has to be a positive number.")]
		public int Ap { get; set; }

		[Display(Name = "Authentication Level")]
		[Required(ErrorMessage = "Auth Level has to be set.")]
		[Remote("IsAuthLevelValid", "Account", ErrorMessage = "AuthLevel invalid.")]
		public AuthLevel AuthLevel { get; set; }

		[Display(Name = "E-Mail Address")]
		[Required(ErrorMessage = "E-Mail is required.")]
		[EmailAddress]
		[Remote("IsEmailUnique", "Account", ErrorMessage = "EMail already registered.", AdditionalFields = nameof(AccountId))]
		public string EMail { get; set; }

		[Display(Name = "Status")]
		[Required(ErrorMessage = "Status has to be set.")]
		[Range(0, short.MaxValue, ErrorMessage = "Status has to be a positive number.")]
		public short Status { get; set; }
	}
}
