using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DALModels.User, BLModels.BLUser>();
            CreateMap<BLModels.BLUser, DALModels.User>();



            CreateMap<DALModels.Country, BLModels.BLCountry>();
            CreateMap<BLModels.BLCountry, DALModels.Country>();

            CreateMap<DALModels.Genre, BLModels.BLGenre>();
            CreateMap<BLModels.BLGenre, DALModels.Genre>();

            CreateMap<DALModels.Image, BLModels.BLImage>();
            CreateMap<DALModels.Notification, BLModels.BLNotification>();


            CreateMap<DALModels.Tag, BLModels.BLTag>();
            CreateMap<BLModels.BLTag, DALModels.Tag>();
            CreateMap<BLModels.BLUser, DALModels.User>();

            CreateMap<DALModels.Video, BLModels.BLVideo>();

            CreateMap<BLModels.BLVideo, DALModels.Video>();


            CreateMap<DALModels.VideoTag, BLModels.BLVideoTag>();

        }
    }
}
