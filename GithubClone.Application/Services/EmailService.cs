using GithubClone.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
         "addyouremailherebruhh@gmail.com",
         "your app password"
     ),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From = new MailAddress("addyouremailbro@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mail.To.Add(new MailAddress(to.Trim()));
            await client.SendMailAsync(mail);
        }
    }
}
