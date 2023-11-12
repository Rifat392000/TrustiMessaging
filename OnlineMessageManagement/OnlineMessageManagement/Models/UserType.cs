using System.ComponentModel.DataAnnotations;
namespace OnlineMessageManagement.Models
{
    public class UserType
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? UserRoll { get; set; }
    }
}
