using System;
using System.Collections.Generic;

namespace AnCoFT.Database.Models
{
	public enum AuthLevel
	{
		None = 0,
		SupportTrainee,
		Support,
		ModeratorTrainee,
		Moderator,
		Admin,
		SuperAdmin,
		Owner = 1337,
	}

	public class Account
    {
		public Account() { }

        public Account(DateTime lastLoginDate)
        {
            this.LastLoginDate = lastLoginDate;
        }

		public Account(string username, string password, DateTime createdAt)
		{
			this.Username = username;
			this.Password = password;
			this.CreationDate = createdAt;
		}

        public int AccountId { get; set; }

        public int Ap { get; set; }

        public List<Character> Characters { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

		public string EMail { get; set; }

		public short Status { get; set; }

		public Home Home { get; set; }

		public bool Premium { get; set; }

		public AuthLevel AuthLevel { get; set; }
		
		public bool Enabled { get; set; }

		public Guid Token { get; set; }
	}
}
