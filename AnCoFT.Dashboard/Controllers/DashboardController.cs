using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AnCoFT.Dashboard.Models;
using AnCoFT.Dashboard.Services;
using AnCoFT.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnCoFT.Dashboard.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly ICharacterService _characterService;

		public DashboardController( IAccountService accountService, ICharacterService characterService)
		{
			_accountService = accountService;
			_characterService = characterService;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Index()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			return View();
		}

		[Authorize(Policy = "Supporter")]
		[HttpGet]
		public IActionResult Accounts(string sortOrder, string currentFilter,
			string searchString, int? pageIndex)
		{
			AccountsModel accounts = new AccountsModel(_accountService.GetAll(), sortOrder,
					currentFilter, searchString, pageIndex);

			return View(accounts);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Characters(string sortOrder, string currentFilter,
			string searchString, int? pageIndex)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			CharactersModel characters;

			if (HttpContext.Request.Cookies.ContainsKey("ViewOwn") || AccountService.GetAuthLevel(User) <= AuthLevel.SupportTrainee)
			{
				characters = new CharactersModel(_characterService.GetByAccountId(AccountService.GetUserId(User)), sortOrder,
					currentFilter, searchString, pageIndex);
			}
			else
			{
				characters = new CharactersModel(_characterService.GetAll(), sortOrder,
					currentFilter, searchString, pageIndex);
			}

			return View(characters);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
