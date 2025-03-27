using System.Net;
using System.Net.Mail;

namespace Company.Kirollos.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            // Mail server : Gmail
            // Protocol SMTP
            // pucauuxycrruxyhz
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("linuxtest612@gmail.com", "pucauuxycrruxyhz");
                client.Send("linuxtest612@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
