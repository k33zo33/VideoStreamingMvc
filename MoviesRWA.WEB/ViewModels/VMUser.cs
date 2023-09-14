using MoviesRWA.BL.DALModels;
using System.ComponentModel;

namespace MoviesRWA.WEB.ViewModels
{
    public class VMUser
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Username { get; set; } = null!;
        [DisplayName("First name")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last name")]
        public string LastName { get; set; } = null!;
        [DisplayName("E-mail")]
        public string Email { get; set; } = null!;
        public string PwdHash { get; set; } = null!;
        public string PwdSalt { get; set; } = null!;
        public string? Phone { get; set; }
        public bool IsConfirmed { get; set; }
        public string? SecurityToken { get; set; }
        public int CountryOfResidenceId { get; set; }
        public virtual Country CountryOfResidence { get; set; } = null!;
    }
}
