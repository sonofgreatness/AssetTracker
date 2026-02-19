using AssetLocater.Domain.Models;
using Microsoft.EntityFrameworkCore;
using AssetLocater.Domain.Security; 

namespace AssetLocater.Domain.Persistence
{



    public class FileDbContext(DbContextOptions<FileDbContext> options) : DbContext(options)
    {
        public DbSet<StoredFile> Files => Set<StoredFile>();
        public DbSet<AppUser> Users => Set<AppUser>();


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
            context.Database.Migrate();

            context.Database.ExecuteSqlRaw(
                "PRAGMA journal_mode=WAL;"
            );

            if (!context.Users.Any())
            {
                context.Users.Add(new AppUser
                {
                    Username = "admin",
                    PasswordHash = PasswordHasher.Hash("admin123"),
                    UserType = UserType.Admin,
                    CreatedAt = DateTime.UtcNow
                });

                context.SaveChanges();
            }


        }


    }
}