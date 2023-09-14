namespace Integration.BLModels
{
    public class BLVideoResp
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int GenreId { get; set; }

        public int TotalTime { get; set; }

        public string? StreamingUrl { get; set; }

        public int? ImageId { get; set; }
    }
}
