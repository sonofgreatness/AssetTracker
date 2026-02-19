namespace AssetLocater.Domain.Models
{
    public sealed class SearchResultBucket
    {
        public string Key { get; init; } = default!;
        public string DisplayName { get; init; } = default!;
        public IReadOnlyList<VehicleRecord> Records { get; init; }
            = Array.Empty<VehicleRecord>();

        public int Count => Records.Count;
    }
}
