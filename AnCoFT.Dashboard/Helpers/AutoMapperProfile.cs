using AnCoFT.Dashboard.Models;
using AnCoFT.Database.Models;
using AutoMapper;

namespace AnCoFT.Dashboard.Helpers
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<RegisterModel, Account>();

			CreateMap<Character, CharacterEdit>();
			CreateMap<CharacterEdit, Character>();

			CreateMap<Account, AccountEdit>();
			CreateMap<Account, AccountEditUser>();
			CreateMap<AccountEdit, Account>();
			CreateMap<AccountEditUser, Account>();
			CreateMap<AccountEdit, AccountEditUser>();
		}
	}
}
