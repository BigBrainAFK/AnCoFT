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
		private readonly Configuration _servConfig;

		public AccountController(IAccountService accountService, IMapper mapper)
		{
			_accountService = accountService;
			_mapper = mapper;
			_servConfig = Configuration.loadConfiguration();
		}

		[AllowAnonymous]
		[HttpPost("login")]
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
		[HttpPost("register")]
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
				ViewData["message"] = ex.Message;
				return View("~/Views/Home/Register.cshtml");
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		[Authorize(Policy = "Supporter")]
		public IActionResult GetAll()
		{
			IEnumerable<Account> accounts = _accountService.GetAll();
			return Ok(accounts);
		}

		[AllowAnonymous]
		[HttpGet("isuserunique")]
		public IActionResult IsUserUnique(string Username)
		{
			return Json(_accountService.IsUserUnique(Username));
		}

		[AllowAnonymous]
		[HttpGet("isemailunique")]
		public IActionResult IsEMailUnique(string EMail)
		{
			return Json(_accountService.IsEMailUnique(EMail));
		}

		public void BuildEmailTemplate(Account account)
		{
			string url = $"https://{_servConfig.url}/Confirm/{account.Token.ToString("D")}";
			string text = string.Format("Please click on this link to confirm your account: {0}", url);
			string html = $"Please confirm your account by clicking this link: <a href=\"{url}\">link</a><br/>";

			html += HttpUtility.HtmlEncode($"Or click on the copy the following link on the browser:{url}");

			MailMessage msg = new MailMessage();
			msg.From = new MailAddress(_servConfig.EMail.EMail);
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
	}
}
