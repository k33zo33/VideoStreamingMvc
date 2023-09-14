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

    public interface ITagRepo
    {
        IEnumerable<BLTag> GetAll();
        BLTag GetById(int id);
        void CreateTag(BLTag tag);
        void UpdateTag(int id, BLTag tag);
        void DeleteTag(int id);
        IEnumerable<BLTag> GetPagedTags(int page, int size);
        int GetTotalCount();

    }

    public class TagRepo : ITagRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public TagRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void CreateTag(BLTag tag)
        {
            try
            {
                var dbTag = _mapper.Map<Tag>(tag);
                _dbContext.Tags.Add(dbTag);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while creating the genre.", e);
            }
        }

        public void DeleteTag(int id)
        {
            try
            {
                var dbTag = _dbContext.Tags.FirstOrDefault(t => t.Id == id);
                if (dbTag == null)
                {

                    throw new ArgumentException("Genre not found.");
                }
                _dbContext.Tags.Remove(dbTag);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while deleting the tag.", e);
            }
        }

        public IEnumerable<BLTag> GetAll()
        {
            try
            {
                var dbTag = _dbContext.Tags;
                var blTag = _mapper.Map<IEnumerable<BLTag>>(dbTag);

                return blTag;
            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while retrieving tags.", e);

            }
        }

        public BLTag GetById(int id)
        {
            try
            {
                var dbTag = _dbContext.Tags.FirstOrDefault(t => t.Id == id);
                if (dbTag == null)
                {

                    throw new ArgumentException("Genre not found.");
                }
                var blTag = _mapper.Map<BLTag>(dbTag);
                return blTag;

            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while retrieving the tag.", e);
            }
        }

        public IEnumerable<BLTag> GetPagedTags(int page, int size)
        {
           
            IEnumerable<Tag> dbTags = _dbContext.Tags.AsEnumerable();

            // Now we can page the correctly ordered items
            dbTags = dbTags.Skip(page * size).Take(size);

            var blTags = _mapper.Map<IEnumerable<BLTag>>(dbTags);

            return blTags;
        }

        public int GetTotalCount() => _dbContext.Tags.Count();

        public void UpdateTag(int id, BLTag tag)
        {
            try
            {

                var dbTag = _dbContext.Tags.FirstOrDefault(t => t.Id == id);
                if (dbTag == null)
                {

                    throw new ArgumentException("Tag not found.");
                }
                dbTag.Name = tag.Name;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {


                throw new Exception("An error occurred while updating the tag.", e);
            }
        }
    }
}
