using Formri.Domain.Entities;
using Formri.Domain.Models.ContactForm;
using Refit;

namespace Formri.Domain.Clients.ContactApi
{
    public interface IApiClient
    {
        /// <summary>
        /// Submits new user form.
        /// </summary>
        [Post("/api/contact/submit")]
        Task SubmitForm([Body] ContactFormModel model);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A list of all users.</returns>
        [Get("/api/users/all")]
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Gets all users with an email address ending in '.biz'.
        /// </summary>
        /// <returns>A list of users with a '.biz' email address.</returns>
        [Get("/api/users/biz")]
        Task<IEnumerable<User>> GetBizUsersAsync();
    }
}
