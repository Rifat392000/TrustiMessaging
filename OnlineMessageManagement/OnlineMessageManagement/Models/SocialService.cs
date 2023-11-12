using System.ComponentModel.DataAnnotations;

namespace OnlineMessageManagement.Models
{
    public class SocialService
    {

        [Key]
        [Display(Name = "Service Id")]
        public int ServiceId { get; set; }

        [Required]
        [Display(Name = "Service Name")]
        public string? ServiceName { get; set; }

        [Required]
        [Display(Name = "Service Status")]
        public int ServiceStatus { get; set; }
    }
}
