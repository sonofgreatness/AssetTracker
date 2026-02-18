using System.ComponentModel.DataAnnotations;

namespace AssetLocater.Domain.Models
{


    public class StoredFile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Required]
        public string FileType { get; set; } = default!;

        public byte[]? Content { get; set; }

        [Required]
        public string ContentType { get; set; } = default!;

        public DateTime CreatedAt { get; set; }
    }

}
