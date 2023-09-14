using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Integration.BLModels
{
    public class BLCreateVideo
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int GenreId { get; set; }

        public int TotalSeconds { get; set; }

        public string? StreamingUrl { get; set; }

        public int? ImageId { get; set; }

        [JsonProperty("tagIds")]
        public int[] TagIds { get; set; } = null!;
    }
}
