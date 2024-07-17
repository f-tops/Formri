namespace Formri.Domain.Services.UserCache
{
    /// <summary>
    /// Interface for user cache operations.
    /// </summary>
    public interface IUserCache
    {
        /// <summary>
        /// Checks if a user has been created recently.
        /// </summary>
        /// <param name="user">The user to check.</param>
        /// <returns>True if the user has been created recently, otherwise false.</returns>
        bool HasUserBeenCreatedRecently(Entities.User user);

        /// <summary>
        /// Adds a user to the cache.
        /// </summary>
        /// <param name="user">The user to add to the cache.</param>
        void AddUserToCache(Entities.User user);
    }
}
