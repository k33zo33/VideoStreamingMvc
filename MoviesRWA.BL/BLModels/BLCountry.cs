using MoviesRWA.BL.DALModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.BLModels
{
    public class BLCountry
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
