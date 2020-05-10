using System;
using System.Threading.Tasks;
using AutoMapper;
using CookbookProject.DataTransferObjects;
using CookbookProject.Models;
using CookbookProject.Services.Repository.Interfaces;
using CookbookProject.Services.Security;
using CookbookProject.Services.EmailClient;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IVerificationRepository verificationRepository;
        private readonly IUserRepository userRepository;
        private readonly IHashConverter hashConverter;
        private readonly ICodeGenerator codeGenerator;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;

        public UserController
        (
            IVerificationRepository verificationRepository,
            IUserRepository userRepository,
            IHashConverter hashConverter,
            ICodeGenerator codeGenerator,
            IEmailSender emailSender,
            IMapper mapper
        )
        {
            this.verificationRepository = verificationRepository;
            this.userRepository = userRepository;
            this.hashConverter = hashConverter;
            this.codeGenerator = codeGenerator;
            this.emailSender = emailSender;
            this.mapper = mapper;
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
        public async Task<VerificationStatus> VerifyUserAsync([FromBody] UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return GetVerificationStatus(400, "Invalid model state");

            var isEmailTaken = await userRepository
                .IsEmailTakenAsync(userViewModel.Email)
                .ConfigureAwait(false);

            if (isEmailTaken)
                return GetVerificationStatus(400, "Email address already in use");

            var isUsernameTaken = await userRepository
                .IsUsernameTakenAsync(userViewModel.Username)
                .ConfigureAwait(false);

            if (isUsernameTaken)
                return GetVerificationStatus(400, "Username already in use");

            var verification = await verificationRepository
                .GetVerificationByUsernameAsync(userViewModel.Username)
                .ConfigureAwait(false);

            if (verification != null)
                return GetVerificationStatus(409, "User conflict occurred");

            var verificationCode = codeGenerator.GetVerificationCode(4);

            try
            {
                await verificationRepository
                    .InsertAsync(new Verification()
                    {
                        Username = userViewModel.Username,
                        Code = verificationCode
                    })
                    .ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                return GetVerificationStatus(500, exc.ToString());
            }

            var response = await emailSender
                .SendAsync(new EmailDetails()
                {
                    ToEmail = userViewModel.Email,
                    ToName = userViewModel.Username,
                    PlainText = $"Verification Code: { verificationCode }"

                })
                .ConfigureAwait(false);

            if (!response.IsValid)
            {
                return await DeleteVerificationIfNecessaryAsync(verification.Id, 500);
            }

            return GetVerificationStatus(200, null, true);
        }

        [HttpPost]
        [Route("Registration")] // api/User/Registration
        public async Task<VerificationStatus> RegisterUserAsync([FromBody] VerificationRequest request)
        {
            if (!ModelState.IsValid)
                return GetVerificationStatus(400, "Invalid model state");

            var verification = await verificationRepository
                .GetVerificationByUsernameAsync(request.UserViewModel.Username)
                .ConfigureAwait(false);

            if (verification.Code != request.VerificationCode)
                return await DeleteVerificationIfNecessaryAsync(verification.Id, 401);

            try
            {
                var user = mapper.Map<User>(request.UserViewModel);
                user.Password = hashConverter.GetHashedPassword(request.UserViewModel.PlainPassword);
                await userRepository
                    .InsertAsync(user)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            {
                return await DeleteVerificationIfNecessaryAsync(verification.Id, 500);
            }

            return GetVerificationStatus(200, null, true);
        }

        [HttpPost]
        [Route("Validation")] // api/User/Validation
        public async Task<bool> ValidateUserAsync([FromBody] Credentials credentials)
        {
            if (!ModelState.IsValid)
                return false;

            return await userRepository.IsUserValidAsync(credentials);
        }

        private VerificationStatus GetVerificationStatus(int statusCode, string errorMessage, bool isValid = false)
        {
            return new VerificationStatus()
            {
                IsValid = isValid,
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            };
        }

        private async Task<VerificationStatus> DeleteVerificationIfNecessaryAsync(int id, int code)
        {
            try
            {
                await verificationRepository
                    .DeleteAsync(id)
                    .ConfigureAwait(false);
            }
            catch (Exception)
            { }

            return GetVerificationStatus(code, "Account verification failed");
        }
    }
}