using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMessageManagement.Models
{
    [Table("ServiceUser")]
    public class ServiceUser
    {
        [Key]
        [Display(Name = "Customer Id")]
        public int Cid { get; set; }

        [Required]
        [StringLength(11)]
        [Display(Name = "Customer Name")]
        public string? Cname { get; set; }


        [Required]
        [StringLength(11)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Services Use")]
        public string? ServiceUse { get; set; }

        [Required]
        [Display(Name = "Create Date and Time")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Update Date and Time")]
        public DateTime UpdatedAt { get; set; }


        [Display(Name = "Customer Id")]
        public int CustomerCid { get; set; } // Added CustomerCid property

        public static implicit operator List<object>(ServiceUser v)
        {
            throw new NotImplementedException();
        }
    }
}
