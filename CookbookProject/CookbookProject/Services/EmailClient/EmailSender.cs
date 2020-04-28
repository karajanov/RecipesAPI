using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookbookProject.Services.EmailClient
{
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; }

        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<EmailResponse> SendAsync(EmailDetails details)
        {
            var apiKey = Configuration
                .GetSection("ApiKeys")
                .GetValue<string>("SendGrid");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(details.FromEmail, details.FromName);
            var subject = details.Subject;
            var to = new EmailAddress(details.ToEmail, details.ToName);
            var plainTextContent = details.PlainText;
            var htmlContent = details.HtmlContent;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                return new EmailResponse()
                {
                    IsValid = true,
                    ErrorMessage = null
                };
            }

            return new EmailResponse()
            {
                IsValid = false,
                ErrorMessage = response.StatusCode.ToString()
            };
        }
    }
}
