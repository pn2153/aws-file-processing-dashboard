using MySql.Data.MySqlClient;
using AwsAssignmentDemo.Models;

namespace AwsAssignmentDemo.Services
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InitializeDatabaseAsync()
        {
            var connectionString =
                _configuration.GetConnectionString("MySqlConnection");

            await using var connection =
                new MySqlConnection(connectionString);

            await connection.OpenAsync();

            // Create database if it doesn't exist
            var createDatabaseSql = @"
                CREATE DATABASE IF NOT EXISTS fileprocessingdb;
            ";

            await using (var dbCommand =
                new MySqlCommand(createDatabaseSql, connection))
            {
                await dbCommand.ExecuteNonQueryAsync();
            }

            // Switch to the new database
            await connection.ChangeDatabaseAsync("fileprocessingdb");

            // Create table if it doesn't exist
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS UploadedFiles
                (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    FileName VARCHAR(255),
                    S3Key VARCHAR(500),
                    UploadDate DATETIME
                );
            ";

            await using (var tableCommand =
                new MySqlCommand(createTableSql, connection))
            {
                await tableCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task SaveUploadAsync(
            string fileName,
            string s3Key)
        {
            var connectionString =
                _configuration.GetConnectionString("MySqlConnection");

            var dbConnectionString =
                $"{connectionString}Database=fileprocessingdb;";

            await using var connection =
                new MySqlConnection(dbConnectionString);

            await connection.OpenAsync();

            var sql = @"
                INSERT INTO UploadedFiles
                (
                    FileName,
                    S3Key,
                    UploadDate
                )
                VALUES
                (
                    @FileName,
                    @S3Key,
                    @UploadDate
                );
            ";

            await using var command =
                new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@FileName", fileName);
            command.Parameters.AddWithValue("@S3Key", s3Key);
            command.Parameters.AddWithValue("@UploadDate", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<UploadedFile>> GetUploadsAsync()
        {
            var uploads = new List<UploadedFile>();

            try
            {
                var connectionString =
                    $"{_configuration.GetConnectionString("MySqlConnection")}Database=fileprocessingdb;";

                await using var connection =
                    new MySqlConnection(connectionString);

                await connection.OpenAsync();

                var sql = @"
                    SELECT
                        Id,
                        FileName,
                        S3Key,
                        UploadDate
                    FROM UploadedFiles
                    ORDER BY UploadDate DESC;
                ";

                await using var command =
                    new MySqlCommand(sql, connection);

                await using var reader =
                    await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    uploads.Add(new UploadedFile
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FileName = reader["FileName"].ToString() ?? "",
                        S3Key = reader["S3Key"].ToString() ?? "",
                        UploadDate = Convert.ToDateTime(reader["UploadDate"])
                    });
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"GetUploadsAsync failed: {ex.Message}");
            }
            return uploads;
        }
    }
}