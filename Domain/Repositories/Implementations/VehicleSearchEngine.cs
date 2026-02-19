using AssetLocater.Domain.Models;
using AssetLocater.Domain.Repositories.Interfaces;

namespace AssetLocater.Domain.Repositories.Implementations
{
    public sealed class VehicleSearchEngine : IVehicleSearchEngine
    {
        private readonly VehicleIndex _index;

        public VehicleSearchEngine(VehicleIndex index)
        {
            _index = index;
        }

        public SearchResult Execute(SearchTerms terms)
        {
            var resultsA = SearchByNationalId(terms.NationalID);
            var resultsB = SearchByName(terms.FullName);
            var resultsC = SearchByCompany(terms.CompanyName);

            return ResultCombiner.Combine(terms, resultsA, resultsB, resultsC);
        }

        private SearchSet SearchByNationalId(string? nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return SearchSet.Empty;

            return _index.ByNationalId.TryGetValue(nationalId, out var list)
                ? new SearchSet(list)
                : SearchSet.Empty;
        }

        private SearchSet SearchByName(string? fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return SearchSet.Empty;

            var tokens = fullName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var results = new HashSet<VehicleRecord>();

            foreach (var token in tokens)
            {
                if (_index.ByNameTokens.TryGetValue(token, out var list))
                {
                    foreach (var record in list)
                        results.Add(record);
                }
            }

            return new SearchSet(results);
        }

        private SearchSet SearchByCompany(string? companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return SearchSet.Empty;

            var results = new HashSet<VehicleRecord>();

            foreach (var ngram in NGramGenerator.Generate(companyName, 4))
            {
                if (_index.ByCompanyNGram.TryGetValue(ngram, out var list))
                {
                    foreach (var record in list)
                        results.Add(record);
                }
            }

            return new SearchSet(results);
        }
    }
}
