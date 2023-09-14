using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using MoviesRWA.Integration.BLModels;

namespace Integration.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Genre, BLGenre>();
            CreateMap<BLGenre, Genre>();
            CreateMap<Genre, BLGenreResp>();
            CreateMap<BLGenreResp, BLGenre>();

            CreateMap<Tag, BLTag>();
            CreateMap<BLTag, Tag>();
            CreateMap<Tag, BLTagResp>();
            CreateMap<BLTagResp, Tag>();

            CreateMap<BLNotificationReq, Notification>();
            CreateMap<Notification, BLNotificationReq>();
            CreateMap<BLNotificationResp, Notification>();
            CreateMap<Notification, BLNotificationResp>();

            CreateMap<Video, BLVideo>();
            CreateMap<BLVideo, Video>();
            CreateMap<Video, BLVideoResp>();
            CreateMap<BLVideoResp, Video>();
            CreateMap<BLCreateVideo,Video>();
            CreateMap<Video, BLCreateVideo>();

            CreateMap<BLUserRegister, User>();
            CreateMap<User, BLUserRegister>();
            CreateMap<BLUserRegisterResp, User>();
            CreateMap<User, BLUserRegisterResp>();
            CreateMap<BLEmailValidationReq, User>();
            CreateMap<User, BLEmailValidationReq>();
            CreateMap<BLUserLogin, User>();
            CreateMap<User, BLUserLogin>();

        }
    }
}
