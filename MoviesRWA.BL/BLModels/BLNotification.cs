using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesRWA.BL.BLModels
{
    public class BLNotification
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ReceiverEmail { get; set; } = null!;

        public string? Subject { get; set; }

        public string Body { get; set; } = null!;

        public DateTime? SentAt { get; set; }
    }
}
