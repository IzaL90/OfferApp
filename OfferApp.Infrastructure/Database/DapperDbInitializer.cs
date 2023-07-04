using FluentMigrator.Runner;
using MySql.Data.MySqlClient;

namespace OfferApp.Infrastructure.Database
{
    internal sealed class DbInitializer : IDbInitializer
    {
        private readonly DatabaseOptions _databaseOptions;
        private readonly IMigrationRunner _migrationRunner;

        public DbInitializer(DatabaseOptions databaseOptions, IMigrationRunner migrationRunner)
        {
            _databaseOptions = databaseOptions;
            _migrationRunner = migrationRunner;
        }

        public void Start()
        {
            CreateDatabaseIfNotExists(_databaseOptions.ConnectionString!);
            _migrationRunner.MigrateUp();
        }

        private static void CreateDatabaseIfNotExists(string connectionString)
        {
            var connectionStringSplited = connectionString.Split(";");
            var connectionStringWithoutDb = connectionStringSplited.Where(str => !str.Contains("Database="))
                                .Aggregate((current, next) => current + ";" + next);
            var database = connectionStringSplited.SingleOrDefault(str => str.Contains("Database="))?.Split("Database=")[1];

            if (string.IsNullOrEmpty(database))
            {
                throw new InvalidOperationException("Invalid ConnectionString. There is no value for 'Database=' check it and try again");
            }

            using var conn = new MySqlConnection(connectionStringWithoutDb);
            using var cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{database}`";
            cmd.ExecuteNonQuery();
        }
    }
}
