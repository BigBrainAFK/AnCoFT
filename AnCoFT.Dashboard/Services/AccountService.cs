using AnCoFT.Dashboard.Helpers;
using AnCoFT.Database;
using AnCoFT.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AnCoFT.Dashboard.Services
{
	public interface IAccountService
	{
		Account Authenticate(string username, string password);
		public bool ConfirmEmail(string token);
		IEnumerable<Account> GetAll();
		Account GetById(int id);
		Account Create(Account account);
		bool IsUserUnique(string Username, int? accountId);
		bool IsEMailUnique(string EMail, int? accountId);
		public void ResetLogin(ClaimsPrincipal User);
	}

	public class AccountService : IAccountService
	{
		DatabaseContext DbContext { get; set; }

		public AccountService(DatabaseContext dbContext)
		{
			this.DbContext = dbContext;
		}

		public Account Authenticate(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				return null;

			Account account = DbContext.Account
					.Include(a => a.Characters)?
					.SingleOrDefault(a => a.Username == username);

			// return null if user not found
			if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password) || !account.Enabled)
				return null;

			account.Password = "";

			// authentication successful so return user details without password
			return account;
		}

		public bool ConfirmEmail(string token)
		{
			Guid tokenGuid = Guid.Parse(token);

			if (!DbContext.Account.Any(a => a.Token == tokenGuid))
				return false;

			Account account = DbContext.Account.First(a => a.Token == tokenGuid);
			account.Enabled = true;
			account.Token = Guid.Empty;

			DbContext.Update(account);
			DbContext.SaveChanges();

			return true;
		}

		public IEnumerable<Account> GetAll()
		{
			IEnumerable<Account> accounts = DbContext.Set<Account>();

			foreach(Account a in accounts)
			{
				a.Password = "";
			}

			return accounts;
		}

		public Account GetById(int id)
		{
			return DbContext.Account.Find(id);
		}

		public Account Create(Account account)
		{
			if (DbContext.Account.Any(a => a.Username == account.Username))
				return null;

			DbContext.Account.Add(account);
			DbContext.SaveChanges();

			return account;
		}

		public static AuthLevel GetAuthLevel(ClaimsPrincipal User)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return AuthLevel.None;
			}

			return (AuthLevel)Enum.Parse(typeof(AuthLevel), User.Claims.First(c => c.Type == "AuthLevel").Value);
		}
		public static int GetUserId(ClaimsPrincipal User)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return 0;
			}

			return int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
		}

		public bool IsUserUnique(string Username, int? accountId)
		{
			if (accountId != null)
			{
				return !DbContext.Account.Any(a => a.Username == Username && a.AccountId != accountId);
			}
			return !DbContext.Account.Any(a => a.Username == Username);
		}

		public bool IsEMailUnique(string EMail, int? accountId)
		{
			if (accountId != null)
			{
				return !DbContext.Account.Any(a => a.EMail == EMail && a.AccountId != accountId);
			}
			return !DbContext.Account.Any(a => a.EMail == EMail);
		}

		public void ResetLogin(ClaimsPrincipal User)
		{
			Account account = GetById(GetUserId(User));

			if (account == null)
			{
				return;
			}

			account.Status = 0;

			DbContext.Update(account);
			DbContext.SaveChanges();
		}
	}
}
