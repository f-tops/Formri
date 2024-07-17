using Formri.Domain.Common.Configurations;
using Formri.Domain.Common.Exceptions;
using Formri.Domain.Services.DatabaseManager;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace Formri.Persistence.Services.DatabaseManager
{
    public class DatabaseManagerService : IDatabaseManagerService
    {
        private readonly string _connectionString;
        private readonly string _masterConnectionString;
        private readonly string _databaseName;
        public DatabaseManagerService(IOptions<DatabaseConfiguration> databaseConfiguration)
        {
            if (string.IsNullOrWhiteSpace(databaseConfiguration.Value.ConnectionString))
            {
                throw new ConfigurationException("Database connection string is missing, please check your configuration.");
            }

            _connectionString = databaseConfiguration.Value.ConnectionString;
            var builder = new SqlConnectionStringBuilder(_connectionString);
            _databaseName = builder.InitialCatalog;

            builder.InitialCatalog = "master";
            _masterConnectionString = builder.ConnectionString;
        }

        public async Task InitializeDatabase(CancellationToken cancellationToken)
        {
            await CreateDatabaseIfNotExists(cancellationToken);
            await CreateTablesAndViewsIfNotExists(cancellationToken);
        }

        private async Task CreateDatabaseIfNotExists(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_masterConnectionString))
            {
                connection.Open();
                var createDatabaseCommand = $@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = N'{_databaseName}')
                BEGIN
                    CREATE DATABASE [{_databaseName}]
                END";
                await ExecuteNonQuery(connection, createDatabaseCommand, cancellationToken);
            }
        }

        private async Task CreateTablesAndViewsIfNotExists(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                string createAddressesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Addresses' AND xtype='U')
                CREATE TABLE Addresses (
                    Id INT PRIMARY KEY IDENTITY,
                    Street NVARCHAR(255),
                    Suite NVARCHAR(255),
                    City NVARCHAR(255),
                    Zipcode NVARCHAR(50),
                    Lat NVARCHAR(255),
                    Lng NVARCHAR(255)
                )";
                await ExecuteNonQuery(connection, createAddressesTable, cancellationToken);

                string createCompaniesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Companies' AND xtype='U')
                CREATE TABLE Companies (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(255),
                    CatchPhrase NVARCHAR(255),
                    Bs NVARCHAR(255)
                )";
                await ExecuteNonQuery(connection, createCompaniesTable, cancellationToken);

                string createUsersTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY,
                    Name NVARCHAR(255),
                    Username NVARCHAR(255),
                    Email NVARCHAR(255),
                    Phone NVARCHAR(50),
                    Website NVARCHAR(255),
                    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                    AddressId INT,
                    CompanyId INT,
                    FOREIGN KEY (AddressId) REFERENCES Addresses(Id),
                    FOREIGN KEY (CompanyId) REFERENCES Companies(Id)
                )";
                await ExecuteNonQuery(connection, createUsersTable, cancellationToken);

                string createBizEmailUsersView = @"
                IF NOT EXISTS (SELECT * FROM sys.views WHERE name='BizEmailUsers')
                BEGIN
                EXEC('CREATE VIEW BizEmailUsers AS
                    SELECT 
                        u.Id AS UserId, u.Name, u.Username, u.Email, u.Phone, u.Website, u.CreatedDate,
                        a.Id AS AddressId, a.Street, a.Suite, a.City, a.Zipcode, a.Lat, a.Lng,
                        c.Id AS CompanyId, c.Name AS CompanyName, c.CatchPhrase, c.Bs
                    FROM Users u
                    JOIN Addresses a ON u.AddressId = a.Id
                    JOIN Companies c ON u.CompanyId = c.Id
                    WHERE u.Email LIKE ''%.biz''')
                END";
                await ExecuteNonQuery(connection, createBizEmailUsersView, cancellationToken);
            }
        }

        private async Task ExecuteNonQuery(SqlConnection connection, string commandText, CancellationToken cancellationToken)
        {
            using (var command = new SqlCommand(commandText, connection))
            {
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}
