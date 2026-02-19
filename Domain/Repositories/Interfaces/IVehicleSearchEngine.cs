using AssetLocater.Domain.Models;

namespace AssetLocater.Domain.Repositories.Interfaces
{
    public interface IVehicleSearchEngine
    {
        SearchResult Execute(SearchTerms terms);
    }
}
