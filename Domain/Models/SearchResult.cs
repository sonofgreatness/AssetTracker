namespace AssetLocater.Domain.Models
{
    public sealed class SearchResult
    {
        /// <summary>
        /// Correlation key (per request / per user).
        /// </summary>
        public string RequestId { get; init; } = default!;

        /// <summary>
        /// Original search input.
        /// </summary>
        public SearchTerms Terms { get; init; } = default!;

        /// <summary>
        /// All result buckets keyed by logical rule name.
        /// </summary>
        public IReadOnlyDictionary<string, SearchResultBucket> Buckets { get; init; }
            = new Dictionary<string, SearchResultBucket>();

        /// <summary>
        /// UTC timestamp of execution.
        /// </summary>
        public DateTime ExecutedAtUtc { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Total unique records across all buckets.
        /// </summary>
        public int TotalUniqueCount { get; init; }
    }
}
