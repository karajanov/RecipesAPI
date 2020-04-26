using System.Threading.Tasks;
using CookbookProject.DataTransferObjects;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProject.Services.Security;
using CookbookProject.Services.Verification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ICodeGenerator codeGenerator;
        private readonly IEmailSender emailSender;

        public UserController(IUserRepository userRepository, ICodeGenerator codeGenerator, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            this.codeGenerator = codeGenerator;
            this.emailSender = emailSender;
        }

        [HttpGet]
        [Route("Availability/Name/{username}")] // api/User/Availability/Name/{username}
        public async Task<bool> IsUsernameTakenAsync([FromRoute] string username)
        {
            return await userRepository
                .IsUsernameTakenAsync(username)
                .ConfigureAwait(false);
        }

        [HttpGet]
        [Route("Availability/Email/{email}")] // api/User/Availability/Email/{email}
        public async Task<bool> IsEmailTakenAsync([FromRoute] string email)
        {
            return await userRepository
                .IsEmailTakenAsync(email)
                .ConfigureAwait(false);
        }

        [HttpPost]
        [Route("Verification")] // api/User/Verification
        public async Task<VerificationResponse> VerifyUserAsync([FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid model state"
                };
            }

            var isEmailTaken = await IsEmailTakenAsync(userViewModel.Email)
                .ConfigureAwait(false);

            if (isEmailTaken)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Email address already in use"
                };
            }

            var isUsernameTaken = await IsUsernameTakenAsync(userViewModel.Username)
                .ConfigureAwait(false);

            if(isUsernameTaken)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Username already in use"
                };
            }

            if(HttpContext.Session.GetString(userViewModel.Email) != null)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    ErrorMessage = "User conflict occurred"
                };
            }

            var verificationCode = codeGenerator.GetVerificationCode(4);
            var verificationResult = new VerificationResult()
            {
                UserViewModel = userViewModel,
                VerificationCode = verificationCode
            };
            HttpContext.Session.SetString(userViewModel.Email, JsonConvert.SerializeObject(verificationResult));

            var response = await emailSender
                 .SendAsync(new EmailDetails()
                 {
                     FromEmail = "karajanov@hotmail.com",
                     FromName = "Cookbook Android App",
                     ToEmail = userViewModel.Email,
                     ToName = userViewModel.Username,
                     Subject = "Verify your account",
                     PlainText = $"Verification Code: { verificationCode }"

                 })
                 .ConfigureAwait(false);

            if(!response.IsValid)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = "Account verification failed"
                };
            }

            return new VerificationResponse()
            {
                IsValid = true,
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPost]
        [Route("Registration")] // api/User/Registration
        public async Task<VerificationResponse> RegisterUserAsync([FromBody] VerificationResult result)
        {
            if(!ModelState.IsValid)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = "Invalid model state"
                };
            }

            var sessionKey = result.UserViewModel.Email;
            var session = HttpContext.Session.GetString(sessionKey);

            if(session == null)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status408RequestTimeout,
                    ErrorMessage = "Session expired"
                };
            }

            var verificationCode = result.VerificationCode;
            var sessionObj = JsonConvert.DeserializeObject<VerificationResult>(session);

            if(sessionObj.VerificationCode != verificationCode)
            {
                return new VerificationResponse()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    ErrorMessage = "Invalid verification code"
                };
            }

            return new VerificationResponse()
            {
                IsValid = true,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
