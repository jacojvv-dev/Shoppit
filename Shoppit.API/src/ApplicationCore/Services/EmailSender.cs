using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ApplicationCore.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken cancellationToken = default);
    }

    public class EmailSender : IEmailSender
    {
        private readonly SendGridOptions _sendGridSettings;

        public EmailSender(IOptions<SendGridOptions> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken cancellationToken = default)
        {
            var client = new SendGridClient(_sendGridSettings.Key);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.From, "Shoppit"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg, cancellationToken);
        }
    }
}