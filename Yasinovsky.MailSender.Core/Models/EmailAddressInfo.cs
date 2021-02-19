using System.ComponentModel.DataAnnotations;

namespace Yasinovsky.MailSender.Core.Models
{
    public abstract class EmailAddressInfo
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public override string ToString() => $"{Name} {Address}";
    }
}