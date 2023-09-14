using Microsoft.AspNetCore.Mvc.Rendering;
using MoviesRWA.BL.DALModels;
using System.ComponentModel;

namespace MoviesRWA.Administration.ViewModels
{
    public class VMVideo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalSeconds { get; set; }
        public int GenreId { get; set; }
       
        public virtual Genre Genre { get; set; } = null!;
        public IFormFile ImageUpload { get; set; }
        public IEnumerable<SelectListItem> Genres { get; set; }
        public int? ImageId { get; set; }
        public string StreamingUrl { get; set; }
    }
}
