using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AnCoFT.Dashboard.Models;
using AnCoFT.Dashboard.Services;
using AnCoFT.Database.Models;
using AnCoFT.Networking.Server;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnCoFT.Dashboard.Controllers
{
	public class DashboardController : Controller
	{
		private readonly IAccountService _accountService;
		private readonly ICharacterService _characterService;
		private readonly Configuration _servConfig;
		private readonly IMapper _mapper;

		public DashboardController( IAccountService accountService, ICharacterService characterService, IMapper mapper)
		{
			_accountService = accountService;
			_characterService = characterService;
			_servConfig = Configuration.loadConfiguration();
			_mapper = mapper;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Index()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			Account account = _accountService.GetById(AccountService.GetUserId(User));
			IEnumerable<Character> characters = _characterService.GetByAccountId(account.AccountId);

			IndexModel indexModel = _mapper.Map<IndexModel>(account);
			indexModel.CharacterStats = _mapper.Map<List<CharacterStatModel>>(characters.ToList());

			return View(indexModel);
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

		[Authorize(Policy = "Admin")]
		[HttpGet]
		public IActionResult EditCharacter(int id)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			if (id <= 0)
			{
				return Redirect(HttpContext.Request.Headers["Referer"]);
			}

			Character character = _characterService.GetById(id);

			if (character == null || AccountService.GetAuthLevel(User) < AuthLevel.Admin)
			{
				return Redirect(HttpContext.Request.Headers["Referer"]);
			}

			CharacterEdit characterEditModel = _mapper.Map<CharacterEdit>(character);
			characterEditModel.Hash = SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(character.CharacterId.ToString() + _servConfig.secret));

			return View(characterEditModel);
		}

		[Authorize]
		[HttpGet]
		public IActionResult EditAccount(int id)
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			if (id <= 0)
			{
				return Redirect(HttpContext.Request.Headers["Referer"]);
			}

			Account account = _accountService.GetById(id);

			if (account == null || (AccountService.GetUserId(User) != account.AccountId && AccountService.GetAuthLevel(User) < AuthLevel.Admin))
			{
				return Redirect(HttpContext.Request.Headers["Referer"]);
			}

			AccountEdit accountEditModel = _mapper.Map<AccountEdit>(account);
			accountEditModel.Hash = SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(account.AccountId.ToString() + _servConfig.secret));

			return View(accountEditModel);
		}

		[Authorize]
		[HttpGet]
		public IActionResult ResetLogin()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Home");
			}

			_accountService.ResetLogin(User);

			return RedirectToAction("Index", "Dashboard");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
