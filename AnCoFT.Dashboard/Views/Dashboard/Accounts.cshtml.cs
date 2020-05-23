using AnCoFT.Dashboard.Helpers;
using AnCoFT.Database.Models;
using Fastenshtein;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnCoFT.Dashboard.Models
{
	public class AccountsModel : PageModel
	{
		public string AccountIdSort { get; set; }
		public string NameSort { get; set; }
		public string ApSort { get; set; }
		public string PremiumSort { get; set; }
		public string DateSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }

		public PaginatedList<Account> Accounts { get; set; }

		public AccountsModel(IEnumerable<Account> accounts, string sortOrder,
			string currentFilter, string searchString, int? pageIndex)
		{
			CurrentSort = sortOrder;
			AccountIdSort = String.IsNullOrEmpty(sortOrder) ? "account_id_desc" : "";
			NameSort = sortOrder == "name" ? "name_desc" : "name";
			ApSort = sortOrder == "ap" ? "ap_desc" : "ap";
			PremiumSort = sortOrder == "premium" ? "premium_desc" : "premium";
			DateSort = sortOrder == "date" ? "date_desc" : "date";

			if (searchString != null)
			{
				pageIndex = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			CurrentFilter = searchString;

			if (!String.IsNullOrEmpty(searchString))
			{
				accounts = accounts.Where(s => Levenshtein.Distance(searchString, s.Username) < 6);
			}

			switch (sortOrder)
			{
				case "account_id_desc":
					accounts = accounts.OrderByDescending(s => s.AccountId);
					break;
				case "name":
					accounts = accounts.OrderBy(s => s.Username);
					break;
				case "name_desc":
					accounts = accounts.OrderByDescending(s => s.Username);
					break;
				case "ap":
					accounts = accounts.OrderBy(s => s.Ap);
					break;
				case "ap_desc":
					accounts = accounts.OrderByDescending(s => s.Ap);
					break;
				case "premium":
					accounts = accounts.OrderBy(s => s.Premium);
					break;
				case "premium_desc":
					accounts = accounts.OrderByDescending(s => s.Premium);
					break;
				case "date":
					accounts = accounts.OrderBy(s => s.LastLoginDate);
					break;
				case "date_desc":
					accounts = accounts.OrderByDescending(s => s.LastLoginDate);
					break;
				default:
					accounts = accounts.OrderBy(s => s.AccountId);
					break;
			}

			int pageSize = 20;
			Accounts = PaginatedList<Account>.Create(
				accounts, pageIndex ?? 1, pageSize);
		}
	}
}
