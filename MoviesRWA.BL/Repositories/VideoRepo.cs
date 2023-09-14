using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Web.Mvc;

namespace MoviesRWA.BL.Repositories
{
    public interface IVideoRepo
    {
        IEnumerable<BLVideo> GetAll();
        BLVideo GetVideoById(int id);
        void CreateVideo(BLVideo video);
        void UpdateVideo(int id, BLVideo video);
        void DeleteVideo(int id);
        IEnumerable<BLVideo> GetPagedVideo(int page, int size, int? selectedVidID, int? selectedGenreId);
        IEnumerable<BLVideo> GetPagedVideoName(int page, int size, string? selectedVideoName);
        int GetTotalCount();
        int GetCountFiltered(int? selectedVidId, int? selectedGenreId);
        int GetNameCountFiltered(string? selectedVidName);
        IEnumerable<BLGenre> GetDataGenreNameFiltered(string term);
        IEnumerable<BLVideo> GetDataNameFiltered(string term);
        BLVideo GetAllGenres();

    }
    public class VideoRepo : IVideoRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public VideoRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void CreateVideo(BLVideo video)
        {
            try
            {

                var dbVideo = _mapper.Map<Video>(video);
                _dbContext.Videos.Add(dbVideo);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while creating the video.", e);
            }
        }

        public void DeleteVideo(int id)
        {
            try
            {
                var dbVideo = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
                if (dbVideo == null)
                {

                    throw new ArgumentException("Video not found.");
                }
                _dbContext.Videos.Remove(dbVideo);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while deleting the video.", e);
            }
        }

        public IEnumerable<BLVideo> GetAll()
        {
            try
            {
                var dbVideos = _dbContext.Videos.Include(v => v.Genre).AsQueryable();

                var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

                return blVideos;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving videos.", e);
            }
        }

        public BLVideo GetAllGenres()
        {
            var genres = _dbContext.Genres.ToList();
            var selectListItems = genres.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            });

            BLVideo blVideo = new BLVideo
            {
                Genres = selectListItems
            };

            _dbContext.SaveChanges();
            return blVideo;
        }

        public BLVideo GetVideoById(int id)
        {
            try
            {
                var dbVideo = _dbContext.Videos.Include(v => v.Genre)
                                  .Where(v => v.Id == id)
                                  .Select(v => new
                                  {
                                      Video = v,
                                      GenreName = v.Genre.Name
                                  })
                                  .FirstOrDefault();

                ;
                if (dbVideo == null)
                {

                    throw new ArgumentException("Video not found.");
                }
                var bLVideo = _mapper.Map<BLVideo>(dbVideo.Video);
                return bLVideo;
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while getting the video.", e);
            }
        }

        public int GetCountFiltered(int? selectedVidId, int? selectedGenreId)
        {
            if (selectedVidId.HasValue)
            {
                return _dbContext.Videos.Where(x => x.Id == selectedVidId.Value).Count();
            }
            if (selectedGenreId.HasValue)
            {
                return _dbContext.Videos.Where(x => x.GenreId == selectedGenreId.Value).Count();
            }
            return _dbContext.Videos.Count();
        }

        public IEnumerable<BLGenre> GetDataGenreNameFiltered(string term)
        {
            var dbGenres = _dbContext.Genres
                  .Where(v => v.Name.Contains(term));

            var bLGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);

            return bLGenres;
        }

        public IEnumerable<BLVideo> GetDataNameFiltered(string term)
        {
            var dbVideos = _dbContext.Videos

                 .Where(v => v.Name.Contains(term));

            var bLVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);

            return bLVideos;
        }

        public int GetNameCountFiltered(string selectedVidName)
        {
            if (!string.IsNullOrEmpty(selectedVidName))
            {
                return _dbContext.Videos.Where(x => x.Name.Contains(selectedVidName)).Count();
            }

            return _dbContext.Videos.Count();
        }

        public IEnumerable<BLVideo> GetPagedVideo(int page, int size, int? selectedVidID, int? selectedGenreId)
        {
            var dbVideos = _dbContext.Videos.Include(v => v.Genre)
                                      .Select(v => new
                                      {
                                          Video = v,
                                          GenreName = v.Genre.Name
                                      })
                                      .AsEnumerable();


            if (selectedVidID.HasValue)
            {
                dbVideos = dbVideos.Where(x => x.Video.Id == selectedVidID.Value);
            }

            if (selectedGenreId.HasValue)
            {
                dbVideos = dbVideos.Where(x => x.Video.GenreId == selectedGenreId.Value);
            }


            dbVideos = dbVideos.Skip(page * size).Take(size);
            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos.Select(v => v.Video));

            return blVideos;
        }

        public IEnumerable<BLVideo> GetPagedVideoName(int page, int size, string? selectedVideoName)
        {
            var dbVideos = _dbContext.Videos.Include(v => v.Genre)
                                   .Select(v => new
                                   {
                                       Video = v,
                                       GenreName = v.Genre.Name
                                   })
                                   .AsEnumerable();

            if (!string.IsNullOrEmpty(selectedVideoName))
            {
                dbVideos = dbVideos.Where(x => x.Video.Name.Contains(selectedVideoName));
            }

            dbVideos = dbVideos.Skip(page * size).Take(size);
            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos.Select(v => v.Video));


            return blVideos;
        }

        public int GetTotalCount() => _dbContext.Videos.Count();
        public void UpdateVideo(int id, BLVideo video)
        {
            try
            {
                var dbVideo = _dbContext.Videos.FirstOrDefault(v => v.Id == id);
                if (dbVideo == null)
                {

                    throw new ArgumentException("Video not found.");
                }
                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.TotalSeconds = video.TotalSeconds;
                dbVideo.StreamingUrl = video.StreamingUrl;
                dbVideo.GenreId = video.GenreId;



                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while updating the video.", e);
            }
        }
    }
}
