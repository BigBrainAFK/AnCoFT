using AnCoFT.Dashboard.Helpers;
using AnCoFT.Database.Models;
using Fastenshtein;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnCoFT.Dashboard.Models
{
	public class CharactersModel : PageModel
	{
		public string CharacterIdSort { get; set; }
		public string NameSort { get; set; }
		public string LevelSort { get; set; }
		public string GoldSort { get; set; }
		public string TypeSort { get; set; }
		public string CurrentFilter { get; set; }
		public string CurrentSort { get; set; }

		public PaginatedList<Character> Characters { get; set; }

		public CharactersModel(IEnumerable<Character> characters, string sortOrder,
			string currentFilter, string searchString, int? pageIndex)
		{
			CurrentSort = sortOrder;
			CharacterIdSort = String.IsNullOrEmpty(sortOrder) ? "character_id_desc" : "";
			NameSort = sortOrder == "name" ? "name_desc" : "name";
			LevelSort = sortOrder == "level" ? "level_desc" : "level";
			GoldSort = sortOrder == "gold" ? "gold_desc" : "gold";
			TypeSort = sortOrder == "type" ? "type_desc" : "type";

			if (searchString != null)
			{
				pageIndex = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			CurrentFilter = searchString;

			if (!String.IsNullOrEmpty(searchString))
			{
				characters = characters.Where(s => Levenshtein.Distance(searchString, s.Name) < 6);
			}

			switch (sortOrder)
			{
				case "character_id_desc":
					characters = characters.OrderByDescending(s => s.CharacterId);
					break;
				case "name":
					characters = characters.OrderBy(s => s.Name);
					break;
				case "name_desc":
					characters = characters.OrderByDescending(s => s.Name);
					break;
				case "level":
					characters = characters.OrderBy(s => s.Level);
					break;
				case "level_desc":
					characters = characters.OrderByDescending(s => s.Level);
					break;
				case "gold":
					characters = characters.OrderBy(s => s.Gold);
					break;
				case "gold_desc":
					characters = characters.OrderByDescending(s => s.Gold);
					break;
				case "type":
					characters = characters.OrderBy(s => s.Type);
					break;
				case "type_desc":
					characters = characters.OrderByDescending(s => s.Type);
					break;
				default:
					characters = characters.OrderBy(s => s.CharacterId);
					break;
			}

			int pageSize = 20;
			Characters = PaginatedList<Character>.Create(
				characters, pageIndex ?? 1, pageSize);
		}
	}
}
