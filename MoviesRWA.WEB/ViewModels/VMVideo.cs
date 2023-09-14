using MoviesRWA.BL.DALModels;

namespace MoviesRWA.WEB.ViewModels
{
    public class VMVideo
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int TotalSeconds { get; set; }

        public virtual Image? Image { get; set; }

        public int? ImageId { get; set; }



    }
}
