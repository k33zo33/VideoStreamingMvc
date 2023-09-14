using AutoMapper;

namespace MoviesRWA.WEB.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, ViewModels.VMUser>();
            CreateMap<ViewModels.VMUser, BL.BLModels.BLUser>();

            CreateMap<BL.BLModels.BLUser, ViewModels.VMRegister>();
            CreateMap<ViewModels.VMRegister, BL.BLModels.BLUser>();

            CreateMap<BL.BLModels.BLUser, ViewModels.VMLogin>();
            CreateMap<ViewModels.VMLogin, BL.BLModels.BLUser>();

            CreateMap<BL.BLModels.BLVideo, ViewModels.VMVideo>();
            CreateMap<ViewModels.VMVideo, BL.BLModels.BLVideo>();

        }
    }
}
