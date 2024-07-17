using Formri.Domain.Models.ContactForm;

namespace Formri.Domain.Services.User
{
    /// <summary>
    /// Interface for user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Adds a contact form user.
        /// </summary>
        /// <param name="contactFormModel">The contact form model containing user data.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the user was successfully added.</returns>
        Task<bool> AddContactFormUser(ContactFormModel contactFormModel, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a user by email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user entity matching the provided email.</returns>
        Task<Entities.User> GetUserByEmail(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all users with an email address ending in '.biz'.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of users with a '.biz' email address.</returns>
        Task<IEnumerable<Entities.User>> GetUsersWithBizEmail(CancellationToken cancellationToken);

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of all users.</returns>
        Task<IEnumerable<Entities.User>> GetAllUsers(CancellationToken cancellationToken);
    }
}
