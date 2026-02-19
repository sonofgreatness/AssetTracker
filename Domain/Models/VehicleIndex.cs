using System.Collections.Concurrent;

namespace AssetLocater.Domain.Models
{

    // <summary>
    /// In-memory search index built from the Vehicles CSV.
    /// Built once and reused for all search requests.
    /// </summary>
    public sealed class VehicleIndex
    {
        // Exact lookup (O(1))
        public ConcurrentDictionary<string, List<VehicleRecord>> ByNationalId { get; }
            = new(StringComparer.Ordinal);

        // Token-based lookup (case-insensitive)
        public ConcurrentDictionary<string, List<VehicleRecord>> ByNameTokens { get; }
            = new(StringComparer.OrdinalIgnoreCase);

        // Fuzzy lookup using n-grams (case-insensitive)
        public ConcurrentDictionary<string, List<VehicleRecord>> ByCompanyNGram { get; }
            = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Clears all indexes (used on reload / refresh).
        /// </summary>
        public void Clear()
        {
            ByNationalId.Clear();
            ByNameTokens.Clear();
            ByCompanyNGram.Clear();
        }
    }

}
