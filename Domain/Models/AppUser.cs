using System.ComponentModel.DataAnnotations;

namespace AssetLocater.Domain.Models
{
    public enum UserType
    {
        Admin = 1,
        Regular = 2
    }

    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = default!;

        [Required]
        public string PasswordHash { get; set; } = default!;

        [Required]
        public UserType UserType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
