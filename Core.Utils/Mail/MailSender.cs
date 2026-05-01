using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;



namespace Core.Utils.Mail
{
    public class MailSender 
    {
        private readonly EmailSettings _emailSettings;
        public MailSender(IOptions<EmailSettings> mailOptions) {
            _emailSettings = mailOptions.Value;
        }
        public async Task SendConfirmationLinkAsync(string username, string email, string confirmationLink)
        {
            using (var c = new SmtpClient())
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("IzyBill", _emailSettings.SendFrom));
                message.To.Add(new MailboxAddress(username, email));
                message.Subject = "Verify Email";
                message.Body = new TextPart("html") { Text = @$"<p>Click <a href='{confirmationLink}'>here</a> to verify email</p>" };

                using (var client = new SmtpClient())
                {
                   // client.IsSecure = true;
                    client.Connect(_emailSettings.Smtp, _emailSettings.Port, SecureSocketOptions.StartTls);
                    
                    ////Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_emailSettings.SendFrom, _emailSettings.Password);

                    var result = client.Send(message);
                    client.Disconnect(true);
                }
            }
        }
        public Task SendPasswordResetCodeAsync(string username, string email, string resetCode)
        {
            throw new NotImplementedException();

        }
        public async Task SendPasswordResetLinkAsync(string username, string email, string resetLink)
        {
            using (var c = new SmtpClient())
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("IzyBill", _emailSettings.SendFrom));
                message.To.Add(new MailboxAddress(username, email));
                message.Subject = "Reset Password";
                message.Body = new TextPart("html") { Text = @$"<p>Click <a href='{resetLink}'>here</a> to reset your password</p>" };

                using (var client = new SmtpClient())
                {
                    // client.IsSecure = true;
                    client.Connect(_emailSettings.Smtp, _emailSettings.Port, SecureSocketOptions.StartTls);

                    ////Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_emailSettings.SendFrom, _emailSettings.Password);

                    var result = client.Send(message);
                    client.Disconnect(true);

                }
            }

        }
    }
}
