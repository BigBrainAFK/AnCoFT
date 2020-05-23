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
		}
	}
}
