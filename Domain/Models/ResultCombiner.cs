namespace AssetLocater.Domain.Models
{
    public static class ResultCombiner
    {
        public static SearchResult Combine(
            SearchTerms terms,
            SearchSet a,
            SearchSet b,
            SearchSet c)
        {
            var buckets = new Dictionary<string, SearchResultBucket>();

            void Add(string key, string label, IEnumerable<VehicleRecord> records)
            {
                var list = records.Distinct().ToList();
                if (list.Count > 0)
                {
                    buckets[key] = new SearchResultBucket
                    {
                        Key = key,
                        DisplayName = label,
                        Records = list
                    };
                }
            }

            Add("1A", "National ID", a.Records);
            Add("1B", "Full Name", b.Records);
            Add("1C", "Company Name", c.Records);

            Add("1A_1B", "National ID + Full Name",
                a.Records.Intersect(b.Records));

            Add("1A_1C", "National ID + Company",
                a.Records.Intersect(c.Records));

            Add("1B_1C", "Full Name + Company",
                b.Records.Intersect(c.Records));

            Add("1A_1B_1C", "All Criteria",
                a.Records.Intersect(b.Records).Intersect(c.Records));

            var uniqueCount = buckets.Values
                .SelectMany(b => b.Records)
                .Distinct()
                .Count();

            return new SearchResult
            {
                RequestId = Guid.NewGuid().ToString("N"),
                Terms = terms,
                Buckets = buckets,
                TotalUniqueCount = uniqueCount
            };
        }
    }
}
