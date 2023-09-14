using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.BLModels
{
    public class BLTag
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
    }
}
