using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineMessageManagement.Models
{
    public class Logpannel
    {
        [Required]
        [MaxLength(250)]
        public string? Name { get; set; }

        [Key]
        public decimal LoginId { get; set; } 
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Password { get; set; }

        [ForeignKey("UserId")]
        public UserType? UserType { get; set; } 
    }
}
