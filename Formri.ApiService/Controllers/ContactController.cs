using Formri.Domain.Models.ContactForm;
using Formri.Domain.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Formri.ApiService.Controllers
{
    /// <summary>
    /// Controller for managing contact form.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Submits new user form.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] ContactFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("All model parameters must have values!");
            }

            var isSuccessful = await userService.AddContactFormUser(model, CancellationToken.None);

            if (isSuccessful)
            {
                return Ok("Form submitted successfully.");
            }

            return BadRequest("An error happened while trying to add new user. A minute must pass until the same user can be resubmitted");
        }
    }
}
