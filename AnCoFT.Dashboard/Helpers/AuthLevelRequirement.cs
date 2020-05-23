using AnCoFT.Database.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnCoFT.Dashboard.Helpers
{
	public class AuthLevelRequirement : IAuthorizationRequirement
	{
		public AuthLevel AuthlevelReq { get; }

		public AuthLevelRequirement(AuthLevel authLevel)
		{
			AuthlevelReq = authLevel;
		}
	}

	public class AuthLevelHander : AuthorizationHandler<AuthLevelRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
													   AuthLevelRequirement requirement)
		{
			if (!context.User.HasClaim(c => c.Type == "AuthLevel"))
			{
				Console.WriteLine(context.User.Claims);
				return Task.CompletedTask;
			}

			Claim authLev = context.User.FindFirst("AuthLevel");

			if ((AuthLevel)Enum.Parse(typeof(AuthLevel), authLev.Value) >= requirement.AuthlevelReq)
			{
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}
}
