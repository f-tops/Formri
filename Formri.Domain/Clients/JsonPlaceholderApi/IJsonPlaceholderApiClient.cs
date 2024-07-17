using Formri.Domain.Models.JsonPlaceholder;
using Refit;

namespace Formri.Domain.Clients.JsonPlaceholderApi
{
    public interface IJsonPlaceholderApiClient
    {
        /// <summary>
        /// Gets all users with specified email.
        /// </summary>
        /// <returns>A list of users with specified mail.</returns>
        [Get("/users")]
        Task<List<JsonPlaceholderUser>> GetUserByEmail([AliasAs("email")] string email);
    }
}
