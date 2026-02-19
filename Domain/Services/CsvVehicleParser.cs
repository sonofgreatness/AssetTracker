using System.Text;
using AssetLocater.Domain.Models;

namespace AssetLocater.Domain.Services
{
    public static class CsvVehicleParser
    {
        public static IEnumerable<VehicleRecord> Parse(byte[] csvBytes)
        {
            using var stream = new MemoryStream(csvBytes);
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: false);

            string? line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var columns = SplitCsvLine(line);

                // We need at least 12 columns AFTER cleaning
                if (columns.Length < 12)
                    continue;

                yield return new VehicleRecord
                {
                    OldNumberPlate = columns[0],
                    NewNumberPlate = columns[1],
                    Description = columns[2],
                    Chassis = columns[3],
                    SpecialNumber = columns[4],
                    OtherSpecialNumber = columns[5],
                    NationalId = NormalizeNationalId(columns[6]),
                    Name = columns[7],
                    Tel = columns[8],
                    Cell = columns[9],
                    PhysicalAddress = $"{columns[10]} {columns[11]}".Trim()
                };
            }
        }

        /// <summary>
        /// Very fast CSV split (handles quoted commas).
        /// </summary>
        private static string[] SplitCsvLine(string line)
        {
            var result = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    result.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            result.Add(sb.ToString().Trim());
            return result.ToArray();
        }

        private static string? NormalizeNationalId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            value = value.Trim();

            return value.Length == 13 && value.All(char.IsDigit)
                ? value
                : null;
        }
    }
}
