using System.ComponentModel.DataAnnotations;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Models
{
    public class Message : IHasId
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required, StringLength(1000)]
        public string Title { get; set; }

        [Required, StringLength(5000)]
        public string Body { get; set; }
    }
}