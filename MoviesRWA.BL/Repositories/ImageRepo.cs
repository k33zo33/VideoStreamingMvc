using AutoMapper;
using Microsoft.AspNetCore.Http;
using MoviesRWA.BL.BLModels;
using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MoviesRWA.BL.Repositories
{
    public interface IImageRepo
    {
        int CreateImage(IFormFile ImgUpload);
        BLImage GetImage(int id);

    }
    public class ImageRepo : IImageRepo
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;

        public ImageRepo(RwaMoviesContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public int CreateImage(IFormFile ImgUpload)
        {
            try
            {


                var imageArray = GetFileByteAray(ImgUpload);

                if (imageArray != null)
                {
                    // To simplify things - we always add image
                    _dbContext.Images.Add(new DALModels.Image()
                    {

                        Content = Convert.ToBase64String(imageArray),

                    });
                }
                _dbContext.SaveChanges();
                return _dbContext.Images.OrderBy(image => image.Id).LastOrDefault().Id;

            }
            catch (Exception e)
            {

                throw new Exception("An error occurred while creating the genre.", e);
            }
        }

        private static byte[] GetFileByteAray(IFormFile image)
        {
            if (image != null)
            {
                if (image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);

                        if (memoryStream.Length < 50 * 1024 * 1024)
                        {
                            return memoryStream.ToArray();
                        }
                    }

                }
            }

            return null;
        }

        public BLImage GetImage(int id)
        {
            var dbFormDataImage = _dbContext.Images.FirstOrDefault(x => x.Id == id);

            if (dbFormDataImage == null)
            {
                return null;
            }

            var blImage = _mapper.Map<BLImage>(dbFormDataImage);


            return blImage;
        }
    }
}
