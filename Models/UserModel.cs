using System.ComponentModel.DataAnnotations;

namespace Be_My_Voice_Backend.Models
{
    public class UserModel
    {
        [Key]
        public Guid UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public string Role { get; set; }

        public string Status { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string PhoneNumber { get; set; }

        public DateOnly DateOfBirth { get; set; }
    }
}
