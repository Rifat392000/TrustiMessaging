
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMessageManagement.Models
{
    public class SingleMessage
    {
        internal int messageStatus;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int logSingleMessage { get; set; }

        [Required]
        public int checkedmsg { get;set; }

        [Required]
        public int Cid { get; set; }

        [Required]
        [StringLength(300)]
        public string? Cname { get; set; }

        [Required]
        [StringLength(11)]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string? ServiceName { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public string? CustomerMessage { get; set; }

        [Required]
        public int MessageStatus { get; set; } = 0;

        [Required]
        public DateTime MsgCreated { get; set; } = DateTime.Now;

        public DateTime MsgSend { get; set; }

     
    }
}
