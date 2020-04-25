using System.Threading.Tasks;
using CookbookProject.DataTransferObjects;
using CookbookProject.Services.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookbookProject.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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

        [HttpPost] // api/User
        public int PostNewUserAsync([FromBody] UserViewModel userViewModel)
        {
            return StatusCodes.Status200OK;
        }


    }
}
