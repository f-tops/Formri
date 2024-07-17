using Formri.Domain.Entities;
using Formri.Domain.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Formri.Api.Controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await userService.GetAllUsers(CancellationToken.None);
            return Ok(users);
        }

        /// <summary>
        /// Gets all users with an email address ending in '.biz'.
        /// </summary>
        /// <returns>A list of users with a '.biz' email address.</returns>
        [HttpGet("biz")]
        public async Task<ActionResult<IEnumerable<User>>> GetBizUsers()
        {
            var users = await userService.GetUsersWithBizEmail(CancellationToken.None);
            return Ok(users);
        }
    }
}
