using System.Net.Mail;

namespace MoviesRWA.WEB.Services
{
    public interface IEmailService
    {
        public void SendEmail(string email, string securityToken);
    }


    public class EmailService : IEmailService
    {
        public void SendEmail(string email, string securityToken)
        {
            SmtpClient client = new SmtpClient("localhost"); //nuget console -> smtp4dev

            string url = $"http://localhost:5099/App/ValidateEmail?email={email}&securitytoken={securityToken}";

            MailAddress from = new MailAddress("test@test.com");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Body = $"Please verify your e-mail with the following link: " + url;
            message.Subject = "Verification e-mail";

            client.Send(message);
        }
    }
}
