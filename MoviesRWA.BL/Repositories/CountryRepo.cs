using AutoMapper;
using Azure;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.Repositories
{
    public interface ICountryRepo
    {
        IEnumerable<BLCountry> GetAll();
        IEnumerable<BLCountry> GetPaged(int page, int size);
        int CountryCount();
    }

    public class CountryRepo : ICountryRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public CountryRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int CountryCount()
        {
            try
            {
                var count = _dbContext.Countries.Count();
                return count;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving the total count of countries.", e);
            }
        }

        public IEnumerable<BLCountry> GetAll()
        {
            try
            {
                var dbCountries = _dbContext.Countries;

                var blCountries = _mapper.Map<IEnumerable<BLCountry>>(dbCountries);

                return blCountries;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while tying to retrieve country data.", e);
            }
        }

        public IEnumerable<BLCountry> GetPaged(int page, int size)
        {
            IEnumerable<Country> dbCountries = _dbContext.Countries.AsEnumerable();

            // Paging corectly ordered items
            dbCountries = dbCountries.Skip(page * size).Take(size);
            var bLCountries = _mapper.Map<IEnumerable<BLCountry>>(dbCountries);

            return bLCountries;
        }
    }
}
