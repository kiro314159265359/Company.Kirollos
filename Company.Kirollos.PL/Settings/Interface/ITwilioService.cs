using Company.Kirollos.PL.Helpers;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Kirollos.PL.Settings.Interface
{
    public interface ITwilioService
    {
        public MessageResource SendSms(SMS sms);
    }
}
