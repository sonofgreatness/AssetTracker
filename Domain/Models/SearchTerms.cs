using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AssetLocater.Domain.Models
{
    public class SearchTerms : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [StringLength(13, MinimumLength = 13,
            ErrorMessage = "National ID must be exactly 13 digits long.")]
        [RegularExpression(@"^\d+$",
            ErrorMessage = "National ID must contain only numbers.")]
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