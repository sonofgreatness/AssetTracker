using System.ComponentModel.DataAnnotations;

namespace AssetLocater.Domain.Models
{
    public class SearchTerms : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        public string? NationalID { get; set; }
        public string? FullName { get; set; }
        public string? CompanyName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(NationalID) &&
                string.IsNullOrWhiteSpace(FullName) &&
                string.IsNullOrWhiteSpace(CompanyName))
            {
                yield return new ValidationResult(
                    "At least one search field must be provided.",
                    new[] { nameof(NationalID), nameof(FullName), nameof(CompanyName) }
                );
            }
        }
    }
}
