using AssetLocater.Domain.Models;

namespace AssetLocater.Domain.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<StoredFile?> GetLatestByTypeAsync(string fileType);
        Task InsertAsync(StoredFile file);
        Task DeleteAsync(int id);
        Task<List<StoredFile>> GetAllAsync();
    }
}
