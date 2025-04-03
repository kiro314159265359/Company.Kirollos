using Company.Kirollos.PL.Helpers;
using Company.Kirollos.PL.Settings.Interface;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace Company.Kirollos.PL.Settings
{
    public class TwilioService(IOptions<TwilioSettings> _options) : ITwilioService
    {
        public MessageResource SendSms(SMS sms)
        {
            // Initialize Connection
            TwilioClient.Init(_options.Value.AccountSID, _options.Value.AuthToken);

            // build message
            var message = MessageResource.Create(
                body: sms.Body,
                from: _options.Value.PhoneNumber,
                to: sms.To
            );

            // return message
            return message;
        }
    }
}
