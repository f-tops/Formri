using Formri.Domain.Services.UserCache;
using Microsoft.Extensions.Caching.Memory;
using Entities = Formri.Domain.Entities;

namespace Formri.Persistence.Services.UserCache
{
    public class UserCache : IUserCache
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

        public UserCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public bool HasUserBeenCreatedRecently(Entities.User user)
        {
            var userExists = _cache.TryGetValue(user.Email, out Entities.User? existingUser);
            if (userExists && existingUser != null)
            {
                if (existingUser.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
                    existingUser.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public void AddUserToCache(Entities.User user)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration);

            _cache.Set(user.Email, user, cacheEntryOptions);
        }
    }
}
