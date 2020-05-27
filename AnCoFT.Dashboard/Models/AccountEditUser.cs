using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

		[Display(Name = "Current Password")]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }

		[Display(Name = "New Password")]
		[StringLength(64, MinimumLength = 8, ErrorMessage = "Password has to be between 8 and 64 characters.")]
		[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "New Password confirmation")]
		[StringLength(64, MinimumLength = 8, ErrorMessage = "Password has to be between 8 and 64 characters.")]
		[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "The passwords do not match.")]
		[NotMapped]
		public string ConfirmPassword { get; set; }
	}
}
