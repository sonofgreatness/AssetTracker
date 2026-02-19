using AssetLocater.Domain.Models;

namespace AssetLocater.Domain.Services
{
    public static class VehicleIndexBuilder
    {
        public static VehicleIndex Build(IEnumerable<VehicleRecord> records)
        {
            var index = new VehicleIndex();

            foreach (var record in records)
            {
                IndexNationalId(index, record);
                IndexNameTokens(index, record);
                IndexCompanyNGrams(index, record);
            }

            return index;
        }

        private static void IndexNationalId(VehicleIndex index, VehicleRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.NationalId))
                return;

            var key = record.NationalId.Trim();

            index.ByNationalId.AddOrUpdate(
                key,
                _ => new List<VehicleRecord> { record },
                (_, list) =>
                {
                    list.Add(record);
                    return list;
                });
        }

        private static void IndexNameTokens(VehicleIndex index, VehicleRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.Name))
                return;

            var tokens = record.Name
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var token in tokens)
            {
                var key = token.ToUpperInvariant();

                index.ByNameTokens.AddOrUpdate(
                    key,
                    _ => new List<VehicleRecord> { record },
                    (_, list) =>
                    {
                        list.Add(record);
                        return list;
                    });
            }
        }

        private static void IndexCompanyNGrams(VehicleIndex index, VehicleRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.Description))
                return;

            var company = record.Description.ToUpperInvariant();

            foreach (var gram in NGramGenerator.Generate(company, 4))
            {
                index.ByCompanyNGram.AddOrUpdate(
                    gram,
                    _ => new List<VehicleRecord> { record },
                    (_, list) =>
                    {
                        list.Add(record);
                        return list;
                    });
            }
        }
    }
}
