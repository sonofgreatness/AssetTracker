namespace AssetLocater.Domain.Models
{
    public static class NGramGenerator
    {
        public static IEnumerable<string> Generate(string input, int size)
        {
            input = input.ToUpperInvariant();

            if (input.Length < size)
                yield break;

            for (int i = 0; i <= input.Length - size; i++)
                yield return input.Substring(i, size);
        }
    }
}
