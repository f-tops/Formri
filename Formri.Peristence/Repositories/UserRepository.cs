using Formri.Domain.Common.Configurations;
using Formri.Domain.Entities;
using Formri.Domain.Repositories.User;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace Formri.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IOptions<DatabaseConfiguration> configuration)
        {
            _connectionString = configuration.Value.ConnectionString;
        }

        public async Task AddUser(User user, CancellationToken cancellationToken, bool existsInJsonPlaceholder = false)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (existsInJsonPlaceholder)
                {
                    var addressId = await InsertAddress(connection, user.Address, cancellationToken);

                    var companyId = await InsertCompany(connection, user.Company, cancellationToken);

                    var command = new SqlCommand(@"
                    INSERT INTO Users (Name, Username, Email, Phone, Website, AddressId, CompanyId, CreatedDate)
                    VALUES (@Name, @Username, @Email, @Phone, @Website, @AddressId, @CompanyId, @CreatedDate)", connection);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Website", user.Website);
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    command.Parameters.AddWithValue("@CompanyId", companyId);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
                else
                {
                    var command = new SqlCommand(@"
                    INSERT INTO Users (Name, Email, CreatedDate)
                    VALUES (@Name, @Email, @CreatedDate)", connection);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.UtcNow);

                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
            }
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(@"
                SELECT u.*, a.*, c.*
                FROM Users u
                LEFT JOIN Addresses a ON u.AddressId = a.Id
                LEFT JOIN Companies c ON u.CompanyId = c.Id
                WHERE u.Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return MapUser(reader);
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"
                    SELECT 
                        u.Id, u.Name, u.Email, u.Phone, u.Website, u.CreatedDate, u.Username,
                        a.Street, a.Suite, a.City, a.Zipcode, a.Lat, a.Lng,
                        c.Name AS CompanyName, c.CatchPhrase, c.Bs
                    FROM Users u
                    LEFT JOIN Addresses a ON u.AddressId = a.Id
                    LEFT JOIN Companies c ON u.CompanyId = c.Id", connection);
                await connection.OpenAsync(cancellationToken);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        var user = MapUser(reader);
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersWithBizEmail(CancellationToken cancellationToken)
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var command = new SqlCommand(@"
                SELECT u.*, a.*, c.*
                FROM BizEmailUsers u
                LEFT JOIN Addresses a ON u.AddressId = a.Id
                LEFT JOIN Companies c ON u.CompanyId = c.Id", connection);

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        users.Add(MapUser(reader));
                    }
                }
            }

            return users;
        }

        private async Task<int> InsertAddress(SqlConnection connection, Address address, CancellationToken cancellationToken)
        {
            var command = new SqlCommand(@"
            INSERT INTO Addresses (Street, Suite, City, Zipcode, Lat, Lng)
            VALUES (@Street, @Suite, @City, @Zipcode, @Lat, @Lng);
            SELECT SCOPE_IDENTITY();", connection);
            command.Parameters.AddWithValue("@Street", address.Street);
            command.Parameters.AddWithValue("@Suite", address.Suite);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@Zipcode", address.Zipcode);
            command.Parameters.AddWithValue("@Lat", address.Lat);
            command.Parameters.AddWithValue("@Lng", address.Lng);

            return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        }

        private async Task<int> InsertCompany(SqlConnection connection, Company company, CancellationToken cancellationToken)
        {
            var command = new SqlCommand(@"
            INSERT INTO Companies (Name, CatchPhrase, Bs)
            VALUES (@Name, @CatchPhrase, @Bs);
            SELECT SCOPE_IDENTITY();", connection);
            command.Parameters.AddWithValue("@Name", company.Name);
            command.Parameters.AddWithValue("@CatchPhrase", company.CatchPhrase);
            command.Parameters.AddWithValue("@Bs", company.Bs);

            return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken));
        }

        private User MapUser(SqlDataReader reader)
        {
            var user = new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? null : reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Website = reader.IsDBNull(reader.GetOrdinal("Website")) ? null : reader.GetString(reader.GetOrdinal("Website")),
                Address = new Address
                {
                    Street = reader.IsDBNull(reader.GetOrdinal("Street")) ? null : reader.GetString(reader.GetOrdinal("Street")),
                    Suite = reader.IsDBNull(reader.GetOrdinal("Suite")) ? null : reader.GetString(reader.GetOrdinal("Suite")),
                    City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                    Zipcode = reader.IsDBNull(reader.GetOrdinal("Zipcode")) ? null : reader.GetString(reader.GetOrdinal("Zipcode")),
                    Lat = reader.IsDBNull(reader.GetOrdinal("Lat")) ? null : reader.GetString(reader.GetOrdinal("Lat")),
                    Lng = reader.IsDBNull(reader.GetOrdinal("Lng")) ? null : reader.GetString(reader.GetOrdinal("Lng"))
                },
                Company = new Company
                {
                    Name = reader.IsDBNull(reader.GetOrdinal("CompanyName")) ? null : reader.GetString(reader.GetOrdinal("CompanyName")),
                    CatchPhrase = reader.IsDBNull(reader.GetOrdinal("CatchPhrase")) ? null : reader.GetString(reader.GetOrdinal("CatchPhrase")),
                    Bs = reader.IsDBNull(reader.GetOrdinal("Bs")) ? null : reader.GetString(reader.GetOrdinal("Bs"))
                }
            };

            return user;
        }
    }
}