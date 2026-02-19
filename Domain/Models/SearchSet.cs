namespace AssetLocater.Domain.Models
{
    public sealed class SearchSet
    {
        public IReadOnlyCollection<VehicleRecord> Records { get; }

        public SearchSet(IEnumerable<VehicleRecord> records)
        {
            Records = records.ToArray();
        }

        public static SearchSet Empty { get; }
            = new SearchSet(Array.Empty<VehicleRecord>());
    }
}
