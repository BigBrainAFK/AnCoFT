using AnCoFT.Database;
using AnCoFT.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnCoFT.Dashboard.Services
{
	public interface ICharacterService
	{
		IEnumerable<Character> GetAll();
		IEnumerable<Character> GetByAccountId(int id);
		public Character GetById(int id);
		public bool IsCharacterNameUnique(string name, int characterId);
	}

	public class CharacterService : ICharacterService
	{
		DatabaseContext DbContext { get; set; }

		public CharacterService(DatabaseContext dbContext)
		{
			this.DbContext = dbContext;
		}

		public IEnumerable<Character> GetAll()
		{
			return DbContext.Set<Character>();
		}

		public IEnumerable<Character> GetByAccountId(int id)
		{
			try
			{
				return DbContext.Character.Where(c => c.AccountId == id).ToList();
			}
			catch
			{
				return new List<Character>();
			}
		}

		public Character GetById(int id)
		{
			try
			{
				return DbContext.Character.Single(c => c.CharacterId == id);
			}
			catch
			{
				return null;
			}
		}

		public bool IsCharacterNameUnique(string name, int characterId)
		{
			return !DbContext.Character.Any(c => c.Name == name && c.CharacterId != characterId);
		}
	}
}
