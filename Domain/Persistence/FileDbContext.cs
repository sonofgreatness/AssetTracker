using AssetLocater.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetLocater.Domain.Persistence
{



    public class FileDbContext(DbContextOptions<FileDbContext> options) : DbContext(options)
    {
        public DbSet<StoredFile> Files => Set<StoredFile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoredFile>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }



    public static class DatabasePath
    {
        public static string Get()
        {
            var basePath = AppContext.BaseDirectory;
            var dataDir = Path.Combine(basePath, "data");

            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            return Path.Combine(dataDir, "assetlocator.db");
        }
    }
    public static class DbInitializer
    {
        public static void Initialize(FileDbContext context)
        {
            context.Database.EnsureCreated();

            context.Database.ExecuteSqlRaw(
                "PRAGMA journal_mode=WAL;"
            );
        }
    }


}
