using AnCoFT.Dashboard.Helpers;
using AnCoFT.Dashboard.Models;
using AnCoFT.Dashboard.Services;
using AnCoFT.Database;
using AnCoFT.Database.Models;
using AnCoFT.Networking.Server;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AnCoFT.Dashboard.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class AccountController : Controller
    {
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		private readonly ICharacterService _characterService;
		private readonly Configuration _servConfig;
		private readonly DatabaseContext _dbContext;

		public AccountController(IAccountService accountService, ICharacterService characterService, IMapper mapper, DatabaseContext dbContext)
		{
			_accountService = accountService;
			_characterService = characterService;
			_mapper = mapper;
			_servConfig = Configuration.loadConfiguration();
			_dbContext = dbContext;
		}

		[AllowAnonymous]
		[HttpPost("Login")]
		[ValidateAntiForgeryToken]
		public IActionResult Login([FromForm]AuthenticateModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/Home/Login.cshtml", model);
			}

			Account account = _accountService.Authenticate(model.Username, model.Password);

			if (account == null)
			{
				ViewData["message"] = "Username or password is incorrect";
				return View("~/Views/Home/Login.cshtml");
			}

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			byte[] key = Encoding.ASCII.GetBytes(_servConfig.secret);
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
					new Claim(ClaimTypes.Name, account.Username),
					new Claim("AuthLevel", account.AuthLevel.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			HttpContext.Session.SetString("JWToken", tokenHandler.WriteToken(token));

			// return basic user info and authentication token
			return RedirectToAction("Index", "Home");
		}

		[AllowAnonymous]
		[HttpPost("Register")]
		[ValidateAntiForgeryToken]
		public IActionResult Register([FromForm]RegisterModel model)
		{
			if (!ModelState.IsValid)
			{
				return View("~/Views/Home/Register.cshtml", model);
			}

			// map model to entity
			Account account = _mapper.Map<Account>(model);

			account.CreationDate = DateTime.Now;

			try
			{
				account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
				account.Token = Guid.NewGuid();

				_accountService.Create(account);

				BuildEmailTemplate(account);
				return View(account);
			}
			catch (AppException ex)
			{
				TempData["message"] = ex.Message;
				return View("~/Views/Home/Register.cshtml", model);
			}
		}

		[Authorize(Policy = "Moderator")]
		[HttpPost("EditCharacter")]
		[ValidateAntiForgeryToken]
		public IActionResult EditCharacter([FromForm]CharacterEdit model)
		{
			if (!ModelState.IsValid)
			{

				Console.WriteLine("not valid");
				return RedirectToAction("EditCharacter", "Dashboard", new { id = model.CharacterId });
			}

			byte[] hash = SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(model.CharacterId.ToString() + _servConfig.secret));

			if (!model.Hash.SequenceEqual(hash))
			{
				TempData["message"] = "CharacterId has been tempered.";
				return RedirectToAction("EditCharacter", "Dashboard", new { id = model.CharacterId });
			}

			try
			{
				Character character = _characterService.GetById(model.CharacterId);

				_mapper.Map(model, character);

				_dbContext.Update(character);
				_dbContext.SaveChanges();

				return RedirectToAction("EditCharacter", "Dashboard", new { id = model.CharacterId });
			}
			catch (AppException ex)
			{
				TempData["message"] = ex.Message;
				return RedirectToAction("EditCharacter", "Dashboard", new { id = model.CharacterId });
			}
		}

		[Authorize]
		[HttpPost("EditAccount")]
		[ValidateAntiForgeryToken]
		public IActionResult EditAccount([FromForm]AccountEdit model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
			}

			byte[] hash = SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(model.AccountId.ToString() + _servConfig.secret));

			if (!model.Hash.SequenceEqual(hash))
			{
				TempData["message"] = "AccountId has been tempered.";
				return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
			}

			try
			{
				Account account = _accountService.GetById(model.AccountId);

				if (AccountService.GetAuthLevel(User) < AuthLevel.Admin)
				{
					AccountEditUser accountEditUser = _mapper.Map<AccountEditUser>(model);

					if (account.Username != accountEditUser.Username)
					{
						TempData["message"] = "You can not edit your username.";
						return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
					}

					if (!String.IsNullOrEmpty(model.CurrentPassword) && !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, account.Password))
					{
						TempData["message"] = "Current password did not match.";
						return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
					}

					model.Password = !String.IsNullOrEmpty(model.CurrentPassword) ? BCrypt.Net.BCrypt.HashPassword(model.Password) : account.Password;

					_mapper.Map(accountEditUser, account);
				}
				else
				{
					if (model.AccountId == AccountService.GetUserId(User))
					{
						if (!String.IsNullOrEmpty(model.CurrentPassword) && !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, account.Password))
						{
							TempData["message"] = "Current password did not match.";
							return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
						}

						model.Password = !String.IsNullOrEmpty(model.CurrentPassword) ? BCrypt.Net.BCrypt.HashPassword(model.Password) : account.Password;
					}
					else
					{
						model.Password = account.Password;
					}

					_mapper.Map(model, account);
				}

				_dbContext.Update(account);
				_dbContext.SaveChanges();

				return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
			}
			catch (AppException ex)
			{
				TempData["message"] = ex.Message;
				return RedirectToAction("EditAccount", "Dashboard", new { id = model.AccountId });
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[Authorize(Policy = "Supporter")]
		[HttpGet]
		public IActionResult GetAll()
		{
			IEnumerable<Account> accounts = _accountService.GetAll();
			return Ok(accounts);
		}

		[AllowAnonymous]
		[HttpGet("IsUserUnique")]
		public IActionResult IsUserUnique(string Username, int? accountId)
		{
			return Json(_accountService.IsUserUnique(Username, accountId));
		}

		[AllowAnonymous]
		[HttpGet("IsEmailUnique")]
		public IActionResult IsEMailUnique(string EMail, int? accountId)
		{
			return Json(_accountService.IsEMailUnique(EMail, accountId));
		}

		public void BuildEmailTemplate(Account account)
		{
			string url = $"https://{_servConfig.url}/Confirm/{account.Token:D}";
			string text = string.Format("Please click on this link to confirm your account: {0}", url);
			string html = $"Please confirm your account by clicking this link: <a href=\"{url}\">link</a><br/>";

			html += HttpUtility.HtmlEncode($"Or click on the copy the following link on the browser:{url}");

			MailMessage msg = new MailMessage
			{
				From = new MailAddress(_servConfig.EMail.EMail)
			};
			msg.To.Add(new MailAddress(account.EMail));
			msg.Subject = "Confirm your account";
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

			SmtpClient smtpClient = new SmtpClient(_servConfig.EMail.SMTP, Convert.ToInt32(_servConfig.EMail.SMTP_Port));
			System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_servConfig.EMail.Username, _servConfig.EMail.Password);
			smtpClient.Credentials = credentials;
			smtpClient.EnableSsl = true;
			smtpClient.Send(msg);
		}

		[Authorize]
		[HttpGet("CharacterIdChanged")]
		public IActionResult CharacterIdChanged(int characterId, byte[] hash)
		{
			return Json(hash.SequenceEqual(SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(characterId.ToString() + _servConfig.secret))));
		}

		[Authorize]
		[HttpGet("AccountIdChanged")]
		public IActionResult AccountIdChanged(int accountId, byte[] hash)
		{
			return Json(hash.SequenceEqual(SHA3.Net.Sha3.Sha3512().ComputeHash(Encoding.UTF8.GetBytes(accountId.ToString() + _servConfig.secret))));
		}

		[Authorize]
		[HttpGet("IsCharacterNameUnique")]
		public IActionResult IsCharacterNameUnique(string name, int characterId)
		{
			return Json(_characterService.IsCharacterNameUnique(name, characterId));
		}

		[Authorize]
		[HttpGet("IsAuthLevelValid")]
		public IActionResult IsAuthLevelValid(int authLevel)
		{
			return Json(Enum.TryParse(typeof(AuthLevel), authLevel.ToString(), out _));
		}
	}
}
