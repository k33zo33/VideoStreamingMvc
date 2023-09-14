using System.ComponentModel;

namespace MoviesRWA.WEB.ViewModels
{
    public class VMLogin
    {

        [DisplayName("Username")]
        public string Username { get; set; }
        [DisplayName("Password")]
        public string Password { get; set; }
        public bool StayLoggedIn { get; set; }

        public string? UrlRedirect { get; set; }

    }
}
