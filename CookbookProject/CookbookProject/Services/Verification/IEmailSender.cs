using System.Threading.Tasks;

namespace CookbookProject.Services.Verification
{
    public interface IEmailSender
    {
        Task<EmailResponse> SendAsync(EmailDetails details);
    }
}
