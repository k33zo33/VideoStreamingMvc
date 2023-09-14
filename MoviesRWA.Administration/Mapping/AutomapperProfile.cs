using AutoMapper;
using MoviesRWA.Administration.ViewModels;
using MoviesRWA.BL.BLModels;

namespace MoviesRWA.Administration.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BLUser, VMUser>();
            CreateMap<VMUser, BLUser>();

            CreateMap<BLTag, VMTag>();
            CreateMap<VMTag, BLTag>();

            CreateMap<BLGenre, VMGenre>();
            CreateMap<VMGenre, BLGenre>();

            CreateMap<BLCountry, VMCountry>();
            CreateMap<VMCountry, BLCountry>();

            CreateMap<BLVideo, VMVideo>();
            CreateMap<VMVideo, BLVideo>();

        }
    }
}
