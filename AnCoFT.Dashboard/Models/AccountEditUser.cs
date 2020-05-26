using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnCoFT.Dashboard.Models
{
	public class AccountEditUser
	{
		[Required]
		[Remote("AccountIdChanged", "Account", ErrorMessage = "The AccountId has been tempered with.", AdditionalFields = nameof(Hash))]
		public int AccountId { get; set; }

		public byte[] Hash { get; set; }

		[Display(Name = "Accountname")]
		public string Username { get; set; }

		[Display(Name = "E-Mail Address")]
		[Required(ErrorMessage = "E-Mail is required.")]
		[EmailAddress]
		[Remote("IsEmailUnique", "Account", ErrorMessage = "EMail already registered.")]
		public string EMail { get; set; }
	}
}
