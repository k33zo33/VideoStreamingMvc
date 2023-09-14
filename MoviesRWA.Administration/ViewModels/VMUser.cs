using System.ComponentModel;

namespace MoviesRWA.Administration.ViewModels
{
    public class VMUser
    {

        public int Id { get; set; }

        [DisplayName("User name")]
        public string Username { get; set; } = null!;
        [DisplayName("First name")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last name")]
        public string LastName { get; set; } = null!;
        public string CountryName { get; set; }
        public int CountryOfResidenceId { get; set; }
        public string? Phone { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
