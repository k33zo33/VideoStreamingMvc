using AutoMapper;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.Repositories
{
    public interface IGenreRepo
    {
        IEnumerable<BLGenre> GetAll();
        BLGenre GetGenre(int id);
        void CreateGenre(BLGenre genre);
        void UpdateGenre(int id, BLGenre genre);
        void DeleteGenre(int id);
        IEnumerable<BLGenre> GetPagedGenres(int page, int size);
        int GetTotalCount();
    }
    public class GenreRepo : IGenreRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public GenreRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void CreateGenre(BLGenre genre)
        {
            try
            {
                var dbGenre = _mapper.Map<Genre>(genre);
                _dbContext.Genres.Add(dbGenre);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while creating the genre.", e);
            }
        }

        public void DeleteGenre(int id)
        {
            var dbGenre = _dbContext.Genres.FirstOrDefault(g => g.Id == id);
            if (dbGenre == null)
            {

                throw new ArgumentException("Genre not found.");
            }

            _dbContext.Genres.Remove(dbGenre);
            _dbContext.SaveChanges();
        }

        public BLGenre GetGenre(int id)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault(g => g.Id == id);
                if (dbGenre == null)
                {

                    throw new ArgumentException("Genre not found.");
                }
                var blGenre = _mapper.Map<BLGenre>(dbGenre);
                return blGenre;
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while retrieving the genre.", e);
            }
        }

        public IEnumerable<BLGenre> GetAll()
        {
            try
            {

                var dbGenres = _dbContext.Genres;


                var blGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);

                return blGenres;
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while retrieving genres.", e);

            }
        }

        public IEnumerable<BLGenre> GetPagedGenres(int page, int size)
        {
            IEnumerable<Genre> dbGenre = _dbContext.Genres.AsEnumerable();

            dbGenre = dbGenre.Skip(page * size).Take(size);

            var blGenre = _mapper.Map<IEnumerable<BLGenre>>(dbGenre);

            return blGenre;
        }

        public int GetTotalCount() => _dbContext.Genres.Count();

        public void UpdateGenre(int id, BLGenre genre)
        {
            try
            {
                var dbGenre = _dbContext.Genres.FirstOrDefault(g => g.Id == id);
                if (dbGenre == null)
                {
                    throw new ArgumentException("Genre not found.");
                }
                dbGenre.Name = genre.Name;
                dbGenre.Description = genre.Description;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while updating the genre.", e);
            }
        }
    }
}
