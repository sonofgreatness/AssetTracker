using AssetLocater.Domain.Models;
using AssetLocater.Domain.Repositories.Interfaces;

namespace AssetLocater.Domain.Services
{
    public class FileService(IFileRepository repo)
    {
        private readonly IFileRepository _repo = repo;

        public Task<StoredFile?> GetVehiclesCsvAsync()
            => _repo.GetLatestByTypeAsync("Vehicles");

        public Task<StoredFile?> GetDeedsCsvAsync()
            => _repo.GetLatestByTypeAsync("Deeds");

        public Task<List<StoredFile>> GetAllAsync()
               => _repo.GetAllAsync();


        public Task InsertAsync(StoredFile file)
              => _repo.InsertAsync(file);

        public Task DeleteAsync(int id)
            => _repo.DeleteAsync(id);


    }
}
