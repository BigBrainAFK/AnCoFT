using System;
using System.Collections.Generic;

namespace AnCoFT.Database
{
    using AnCoFT.Database.Models;
    using AnCoFT.Game.Guild;
    using Microsoft.EntityFrameworkCore;

	public class DatabaseContext : DbContext
    {
        public AnCoFT.Networking.Server.DatabaseConfiguration dbConfig;

        public DbSet<Account> Account { get; set; }

		public DbSet<Challenge> Challenge { get; set; }

        public DbSet<ChallengeProgress> ChallengeProgress { get; set; }

        public DbSet<Tutorial> Tutorial { get; set; }

        public DbSet<TutorialProgress> TutorialProgress { get; set; }

        public DbSet<Character> Character { get; set; }
        public DbSet<CharacterInventory> CharacterInventory { get; set; }

        public DbSet<GameServer> GameServer { get; set; }

        public DbSet<Home> Home { get; set; }
        public DbSet<HomeInventory> HomeInventory { get; set; }
        public DbSet<Guild> Guild { get; set; }
        public DbSet<GuildMember> GuildMember { get; set; }
        public DbSet<GuildGoldUsage> GuildGoldUsage { get; set; }
        public DbSet<MessengerFriend> MessengerFriendList { get; set; }
        public DbSet<MessengerMessage> MessengerMessage { get; set; }
        public DbSet<MessengerParcel> MessengerParcel { get; set; }
        public DbSet<MessengerProposal> MessengerProposal { get; set; }

		public DatabaseContext()
		{
			if (dbConfig == null)
			{
				AnCoFT.Networking.Server.Configuration temp = AnCoFT.Networking.Server.Configuration.loadConfiguration();

				this.dbConfig = temp.dbConfig;
			}
		}

		public DatabaseContext(string configPath) : this()
		{
			AnCoFT.Networking.Server.Configuration temp = AnCoFT.Networking.Server.Configuration.loadConfiguration(configPath);

			this.dbConfig = temp.dbConfig;
		}

        public DatabaseContext(AnCoFT.Networking.Server.DatabaseConfiguration dbConfig) : this()
        {
            this.dbConfig = dbConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={this.dbConfig.ip};Port={this.dbConfig.port};Database={this.dbConfig.database};Username={this.dbConfig.username};{(this.dbConfig.password.Length > 0 ? $"Password={this.dbConfig.password};" : "")}");
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<TutorialProgress>().HasKey(t => new { t.CharacterId, t.TutorialId });
            modelBuilder.Entity<ChallengeProgress>().HasKey(c => new { c.CharacterId, c.ChallengeId });
            modelBuilder.Entity<GuildMember>().HasKey(gm =>  gm.CharacterId);
            modelBuilder.Entity<MessengerFriend>().HasKey(mf => new { mf.CharacterId, mf.FriendCharacterId });

            modelBuilder.Entity<Account>().Property(a => a.AccountId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Character>().Property(c => c.CharacterId).ValueGeneratedOnAdd();
            modelBuilder.Entity<CharacterInventory>().Property(ci => ci.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GameServer>().Property(gs => gs.GameServerId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Guild>().Property(g => g.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GuildGoldUsage>().Property(ggu => ggu.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Home>().Property(h => h.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<HomeInventory>().Property(hi => hi.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<MessengerMessage>().Property(mm => mm.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MessengerParcel>().Property(mp => mp.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<MessengerProposal>().Property(mp => mp.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Tutorial>().Property(t => t.TutorialId).ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Account>().Property(a => a.AccountId).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<Character>().Property(c => c.CharacterId).HasIdentityOptions(startValue: 3);
            modelBuilder.Entity<GameServer>().Property(gs => gs.GameServerId).HasIdentityOptions(startValue: 2);
            modelBuilder.Entity<Guild>().Property(g => g.Id).HasIdentityOptions(startValue: 2);
            

            // Debug purpose
            modelBuilder.Entity<GameServer>().HasData(new List<GameServer>()
            {
                new GameServer()
                {
                    GameServerId = 1, Host = "127.0.0.1", Port = 5895, ServerType = GameServerType.Free
                }
            });

			modelBuilder.Entity<Account>().HasData(new List<Account>()
			{
				new Account(DateTime.MinValue)
				{
					AccountId = 1, Ap = 0, Status = 0, Username = "ancoft", Password = BCrypt.Net.BCrypt.HashPassword("ancoft"), EMail = "email@example.com", AuthLevel = AuthLevel.None, Enabled = true, Premium = false
				},
				new Account(DateTime.MinValue)
				{
					AccountId = 2, Ap = 0, Status = 0, Username = "ancoft2", Password = BCrypt.Net.BCrypt.HashPassword("ancoft2"), EMail = "email@example.com", AuthLevel = AuthLevel.Owner, Enabled = true, Premium = true
				}
			});

            modelBuilder.Entity<Character>().HasData(new List<Character>()
            {
                new Character() {AccountId = 1, CharacterId = 1, Name = "AnCoFT", Gold = 20000, AlreadyCreated = true, Type = 0},
                new Character() {AccountId = 2, CharacterId = 2, Name = "AnCoFT2", Gold = 20000, AlreadyCreated = true, Type = 1}
            });

            modelBuilder.Entity<Guild>().HasData(new List<Guild>()
            {
                new Guild()
                {
                    Id = 1, Name = "AnCoGuild", BattleRecordLoose = 0, BattleRecordWin = 0, ClubGold = 5000, ClubPoints = 0, CreationDay = DateTime.MinValue, Emblem = 0, 
                    Public = true, LeagueRecordLoose = 0, LeagueRecordWin = 0, LeaguePoints = 0, Level = 1, LevelRestriction = 0, MaxMemberCount = 50,
                    Introduction = "", Notice = ""
                }
            });

            modelBuilder.Entity<GuildMember>().HasData(new List<GuildMember>()
            {
                new GuildMember()
                {
                    CharacterId = 1, Division = (GuildDivision)3, ContributionPoints = 0, RequestDate = DateTime.MinValue, WaitingForApproval = false, GuildId = 1
                },
                new GuildMember()
                {
                    CharacterId = 2, Division =  (GuildDivision)2, ContributionPoints = 0, RequestDate = DateTime.MinValue, WaitingForApproval = false, GuildId = 1
                }
            });
        }
    }
}
