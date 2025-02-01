using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.Mail
{
    public class MailSender : IEmailSender<IdentityUser>
    {
        private readonly EmailSettings _emailSettings;
       // private readonly 
       public MailSender(IOptions<EmailSettings> mailOptions) {
            _emailSettings = mailOptions.Value;
           // MailKit.
       }
        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
    }
}
