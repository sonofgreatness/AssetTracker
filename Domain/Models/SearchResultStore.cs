using Microsoft.Extensions.Caching.Memory;

namespace AssetLocater.Domain.Models
{
    public sealed class SearchResultStore
    {
        private readonly IMemoryCache _cache;

        public string Store(SearchResult result)
        {
            var key = Guid.NewGuid().ToString("N");
            _cache.Set(key, result, TimeSpan.FromMinutes(10));
            return key;
        }

        public SearchResult? Get(string key)
            => _cache.TryGetValue(key, out SearchResult r) ? r : null;
    }
}
