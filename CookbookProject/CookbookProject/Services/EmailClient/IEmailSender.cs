using System.Threading.Tasks;

namespace CookbookProject.Services.EmailClient
{
    public interface IEmailSender
    {
        Task<EmailResponse> SendAsync(EmailDetails details);
    }
}
