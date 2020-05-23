using AnCoFT.Dashboard.Helpers;
using AnCoFT.Dashboard.Services;
using AnCoFT.Database;
using AnCoFT.Database.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace AnCoFT.Dashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

			Console.Write("Reading in config files... ");
			ServConfig = Networking.Server.Configuration.loadConfiguration();
			if (ServConfig == null)
			{
				return;
			}
			Console.WriteLine("OK!");

			Console.Write("Initializing Database... ");
			DatabaseContext dbContext = new DatabaseContext(ServConfig.dbConfig);
			try
			{
				dbContext.Database.EnsureDeleted();
				dbContext.Database.EnsureCreated();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error occured while initializing database [{ex.Message}]");
				Console.WriteLine(ex.StackTrace);
				Console.WriteLine("Press any key to exit...");
				Console.ReadLine();
				return;
			}
			Console.WriteLine("OK!");
		}

        public IConfiguration Configuration { get; }
		public Networking.Server.Configuration ServConfig { get; }
		public DatabaseContext DbContext { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(60);
			});

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddSingleton<IAuthorizationHandler, AuthLevelHander>();

			// Configure your policies
			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin",
				policy =>
					policy.Requirements.Add(new AuthLevelRequirement(AuthLevel.Admin))
				);

				options.AddPolicy("Supporter",
				policy =>
					policy.Requirements.Add(new AuthLevelRequirement(AuthLevel.Support))
				);
			});

			// configure jwt authentication
			var key = Encoding.ASCII.GetBytes(ServConfig.secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.TokenValidationParameters = new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
				};
			});

			services.AddControllersWithViews();

			services.AddDbContext<DatabaseContext>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICharacterService, CharacterService>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseSession();

			app.Use(async (context, next) =>
			{
				byte[] JWTokenBytes = context.Session.Get("JWToken");

				if (JWTokenBytes != null && JWTokenBytes.Length > 0)
				{
					context.Request.Headers.Add("Authorization", "Bearer " + Encoding.Default.GetString(JWTokenBytes));
				}
				await next();
			});

			app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{action=Index}/{guid?}",
					defaults: new { controller = "Home" });

				endpoints.MapControllerRoute(
                    name: "normal",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
			});
        }
    }
}
