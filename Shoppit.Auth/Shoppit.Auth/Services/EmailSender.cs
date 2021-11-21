using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Shoppit.Auth.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Shoppit.Auth.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridSettings _sendGridSettings;

        public EmailSender(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_sendGridSettings.Key);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.From, "Password Recovery"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}