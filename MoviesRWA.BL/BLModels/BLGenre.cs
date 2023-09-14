using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.BLModels
{
    public class BLGenre
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
