using AnCoFT.Database;
using AnCoFT.Database.Models;
using System.Collections.Generic;
using System.Linq;

namespace AnCoFT.Dashboard.Services
{
	public interface ICharacterService
	{
		IEnumerable<Character> GetAll();
		IEnumerable<Character> GetByAccountId(int id);
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
			return DbContext.Character.Where(c => c.AccountId == id).ToList();
		}
	}
}
