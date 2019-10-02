namespace AnCoFT.Database.Models
{
    using System;
    using System.Collections.Generic;

    public class Account
    {
        public Account(DateTime lastLoginDate)
        {
            this.LastLoginDate = lastLoginDate;
        }

        public int AccountId { get; set; }

        public int Ap { get; set; }

        public List<Character> Characters { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public short Status { get; set; }
    }
}