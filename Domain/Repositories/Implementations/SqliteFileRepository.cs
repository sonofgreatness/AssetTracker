
using AssetLocater.Domain.Models;
using AssetLocater.Domain.Persistence;
using AssetLocater.Domain.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;
namespace AssetLocater.Domain.Repositories.Implementations
{

    public class SqliteFileRepository(FileDbContext context) : IFileRepository
    {
        private readonly FileDbContext _context = context;
        public async Task<List<StoredFile>> GetAllAsync()
        {
            return await _context.Files
                .AsNoTracking()
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new StoredFile
                {
                    Id = f.Id,
                    Name = f.Name,
                    FileType = f.FileType,
                    ContentType = f.ContentType,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }
        public async Task<StoredFile?> GetLatestByTypeAsync(string fileType)
        {
            return await _context.Files
                .AsNoTracking()
                .Where(f => f.FileType == fileType)
                .OrderByDescending(f => f.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task InsertAsync(StoredFile file)
        {
            file.CreatedAt = DateTime.UtcNow;
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Files.FindAsync(id);
            if (entity == null) return;

            _context.Files.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }


}
