﻿using Company.Kirollos.PL.Helpers;
using Company.Kirollos.PL.Settings.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Company.Kirollos.PL.Settings
{
    public class MailService(IOptions<MailSettings> _options) : IMailService
    {
        public void SendEmail(Email email)
        {
            // Build Message
            var mail = new MimeMessage();
            mail.Subject = email.Subject;
            mail.From.Add(new MailboxAddress(_options.Value.DisplayName ,_options.Value.Email));
            mail.To.Add(MailboxAddress.Parse(email.To));

            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Establish Connection
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host , _options.Value.Port , MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Email , _options.Value.Password);

            // Send Message
            smtp.Send(mail);
        }
    }
}
