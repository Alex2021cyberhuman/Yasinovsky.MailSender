using System.ComponentModel.DataAnnotations;

namespace Yasinovsky.MailSender.Core.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required, StringLength(1000)]
        public string Title { get; set; }

        [Required, StringLength(5000)]
        public string Body { get; set; }
    }
}