using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.BLModels
{
    public class BLImage
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
