namespace Formri.Domain.Services.DatabaseManager
{
    /// <summary>
    /// Singleton service responsible for initializing database data
    /// </summary>
    public interface IDatabaseManagerService
    {
        /// <summary>
        /// Initializes the database by creating the database, tables, and views if they do not already exist.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task InitializeDatabase(CancellationToken cancellationToken);
    }
}
