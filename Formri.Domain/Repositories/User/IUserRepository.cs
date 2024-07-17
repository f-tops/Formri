namespace Formri.Domain.Repositories.User
{
    public interface IUserRepository
    {
        public Task AddUser(Entities.User user, CancellationToken cancellationToken, bool existsInJsonPlaceholder = false);

        public Task<Entities.User> GetUserByEmail(string email, CancellationToken cancellationToken);

        public Task<IEnumerable<Entities.User>> GetUsersWithBizEmail(CancellationToken cancellationToken);

        public Task<IEnumerable<Entities.User>> GetAllUsers(CancellationToken cancellationToken);

    }
}
