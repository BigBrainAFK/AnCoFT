using System;
using System.Collections.Generic;

namespace AnCoFT.Database
{
    using AnCoFT.Database.Models;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
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


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=localhost;Port=5439;Database=AnCoFT;Username=postgres;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TutorialProgress>().HasKey(t => new { t.CharacterId, t.TutorialId });
            modelBuilder.Entity<ChallengeProgress>().HasKey(c => new { c.CharacterId, c.ChallengeId });
            modelBuilder.Entity<GuildMember>().HasKey(gm =>  gm.CharacterId);
            modelBuilder.Entity<MessengerFriend>().HasKey(mf => new { mf.CharacterId, mf.FriendCharacterId });

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
                    AccountId = 1, Ap = 0, Status = 0, Login = "ancoft", Password = "ancoft"
                },
                new Account(DateTime.MinValue)
                {
                    AccountId = 2, Ap = 0, Status = 0, Login = "ancoft2", Password = "ancoft2"
                }
            });
            modelBuilder.Entity<Character>().HasData(new List<Character>()
            {
                new Character() {AccountId = 1, CharacterId = 1, Name = "AnCoFT", Gold = 20000, AlreadyCreated = true},
                new Character() {AccountId = 2, CharacterId = 2, Name = "AnCoFT2", Gold = 20000, AlreadyCreated = true}
            });
            modelBuilder.Entity<Guild>().HasData(new List<Guild>()
            {
                new Guild()
                {
                    ID = 1, Name = "AnCoGuild", BattleRecordLoose = 0, BattleRecordWin = 0, ClubGold = 5000, ClubPoints = 0, CreationDay = 0, Emblem = 0, 
                    Public = true, LeagueRecordLoose = 0, LeagueRecordWin = 0, LeaguePoints = 0, Level = 1, LevelRestriction = 0, MaxMemberCount = 50,
                    Introduction = "", Notice = ""
                }
            });
            modelBuilder.Entity<GuildMember>().HasData(new List<GuildMember>()
            {
                new GuildMember()
                {
                    CharacterId = 1, Division = 3, ContributionPoints = 0, RequestDate = 0, WaitingForApproval = false, GuildId = 1
                },
                new GuildMember()
                {
                    CharacterId = 2, Division = 2, ContributionPoints = 0, RequestDate = 0, WaitingForApproval = false, GuildId = 1
                }
            });
        }
    }
}