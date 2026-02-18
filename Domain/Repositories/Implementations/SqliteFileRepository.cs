
using AssetLocater.Domain.Repositories.Interfaces;
using AssetLocater.Domain.Models;
using Microsoft.Data.Sqlite;

namespace AssetLocater.Domain.Repositories.Implementations
{

    public class SqliteFileRepository : IFileRepository
    {
        private readonly string _connectionString;

        public SqliteFileRepository()
        {
            var basePath = AppContext.BaseDirectory;
            var dataDir = Path.Combine(basePath, "data");

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            var dbPath = Path.Combine(dataDir, "assetlocator.db");

            _connectionString = $"Data Source={dbPath};Cache=Shared";
        }

        // -----------------------------
        // Connection bootstrap helpers
        // -----------------------------

        private async Task<SqliteConnection> OpenConnectionAsync()
        {
            var conn = new SqliteConnection(_connectionString);
            await conn.OpenAsync();

            // Enable WAL for concurrency
            using (var walCmd = conn.CreateCommand())
            {
                walCmd.CommandText = "PRAGMA journal_mode=WAL;";
                await walCmd.ExecuteNonQueryAsync();
            }

            // Ensure schema exists
            using (var schemaCmd = conn.CreateCommand())
            {
                schemaCmd.CommandText = """
                    CREATE TABLE IF NOT EXISTS Files (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        FileType TEXT NOT NULL,
                        Content BLOB NOT NULL,
                        ContentType TEXT NOT NULL,
                        CreatedAt TEXT NOT NULL
                    );
                """;
                await schemaCmd.ExecuteNonQueryAsync();
            }

            return conn;
        }

        // -----------------------------
        // Repository methods
        // -----------------------------

        public async Task<StoredFile?> GetLatestByTypeAsync(string fileType)
        {
            using var conn = await OpenConnectionAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                SELECT Id, Name, FileType, Content, ContentType
                FROM Files
                WHERE FileType = @type
                ORDER BY CreatedAt DESC
                LIMIT 1
            """;
            cmd.Parameters.AddWithValue("@type", fileType);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read())
                return null;

            return new StoredFile
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                FileType = reader.GetString(2),
                Content = (byte[])reader["Content"],
                ContentType = reader.GetString(4)
            };
        }

        public async Task InsertAsync(StoredFile file)
        {
            using var conn = await OpenConnectionAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
                INSERT INTO Files (Name, FileType, Content, ContentType, CreatedAt)
                VALUES (@name, @type, @content, @contentType, datetime('now'))
            """;

            cmd.Parameters.AddWithValue("@name", file.Name);
            cmd.Parameters.AddWithValue("@type", file.FileType);
            cmd.Parameters.AddWithValue("@content", file.Content);
            cmd.Parameters.AddWithValue("@contentType", file.ContentType);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var conn = await OpenConnectionAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Files WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            await cmd.ExecuteNonQueryAsync();
        }






        public async Task<List<StoredFile>> GetAllAsync()
        {
            using var conn = await OpenConnectionAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = """
        SELECT Id, Name, FileType, ContentType, CreatedAt
        FROM Files
        ORDER BY CreatedAt DESC
    """;

            using var reader = await cmd.ExecuteReaderAsync();
            var result = new List<StoredFile>();

            while (reader.Read())
            {
                result.Add(new StoredFile
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    FileType = reader.GetString(2),
                    ContentType = reader.GetString(3),
                    CreatedAt = reader.GetString(4)
                });
            }

            return result;
        }
    }
}
