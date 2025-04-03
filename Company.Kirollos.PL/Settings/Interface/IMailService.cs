using Company.Kirollos.PL.Helpers;

namespace Company.Kirollos.PL.Settings.Interface
{
    public interface IMailService
    {
        public void SendEmail(Email email);
    }
}
