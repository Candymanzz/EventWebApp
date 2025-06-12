using EventWebApp.Application.Interfaces;
using EventWebApp.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EventWebApp.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly SmtpSettings smtpSettings;

        public EmailNotificationService(IOptions<SmtpSettings> smtpOptions)
        {
            smtpSettings = smtpOptions.Value;
        }

        public async Task NotifyUsersAsync(
            IEnumerable<string> userEmails,
            string subject,
            string message
        )
        {
            foreach (var email in userEmails.Distinct())
            {
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(MailboxAddress.Parse(smtpSettings.Sender));
                mimeMessage.To.Add(MailboxAddress.Parse(email));
                mimeMessage.Subject = subject;
                mimeMessage.Body = new TextPart("plain") { Text = message };

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    smtpSettings.Host,
                    smtpSettings.Port,
                    SecureSocketOptions.StartTls
                );
                await client.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
                await client.SendAsync(mimeMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
