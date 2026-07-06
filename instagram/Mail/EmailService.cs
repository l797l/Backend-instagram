using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace instagram.Mail
{
    public class EmailService : IEmailService
    {
        readonly MailSettings mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            this.mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    mailSettings.DisplayName,
                    mailSettings.From));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(
                mailSettings.Host,
                mailSettings.Port,
                SecureSocketOptions.StartTls);


            await smtp.AuthenticateAsync(
                mailSettings.UserName,
                mailSettings.Password);
            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }

    }
}
