using System.ComponentModel.DataAnnotations;

namespace Integration.BLModels
{
    public class BLNotificationResp
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string ReceiverEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}
