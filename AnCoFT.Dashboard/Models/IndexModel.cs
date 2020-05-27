using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AnCoFT.Dashboard.Models
{
	public class IndexModel
	{
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Display(Name = "Premium")]
		public bool Premium { get; set; }

		[Display(Name = "AP")]
		public int Ap { get; set; }

		public List<CharacterStatModel> CharacterStats { get; set; }
	}

	public class CharacterStatModel
	{
		[Display(Name = "´Level")]
		public byte Level { get; set; }

		[Display(Name = "Experience points")]
		public int Exp { get; set; }

		[Display(Name = "Gold")]
		public int Gold { get; set; }

		[Display(Name = "Battles won")]
		public int BattlesWon { get; set; }

		[Display(Name = "Battles lost")]
		public int BattlesLost { get; set; }
	}
}
