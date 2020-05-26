using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnCoFT.Dashboard.Models;
using AnCoFT.Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using System;

namespace AnCoFT.Dashboard.Controllers
{
    public class HomeController : Controller
    {
		private readonly IAccountService _accountService;

        public HomeController(IAccountService accountService)
        {
			_accountService = accountService;
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Index()
        {
            return View();
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Privacy()
        {
            return View();
		}

		[Authorize]
		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("JWToken");
			return RedirectToAction("Index");
		}

		[AllowAnonymous]
		[HttpGet]
		public IActionResult Confirm(string guid)
		{
			bool converted = Guid.TryParse(guid, out _);

			if (!converted)
			{
				return RedirectToAction("Index");
			}

			if (_accountService.ConfirmEmail(guid))
			{
				return View();
			}
			else
			{
				return RedirectToAction("Index");
			}
		}

		public IActionResult SetDark()
		{
			Response.Cookies.Append("ColorMode", "DarkMode", new Microsoft.AspNetCore.Http.CookieOptions { Expires = null });
			return Redirect(HttpContext.Request.Headers["Referer"]);
		}

		public IActionResult SetLight()
		{
			Response.Cookies.Append("ColorMode", "LightMode", new Microsoft.AspNetCore.Http.CookieOptions { Expires = null });
			return Redirect(HttpContext.Request.Headers["Referer"]);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
