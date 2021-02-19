using System.ComponentModel.DataAnnotations;

namespace Yasinovsky.MailSender.Core.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(500)] 
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Address { get; set; }

        public int Port { get; set; } = 25;

        public bool EnableSsl { get; set; } = true;

        [Required, StringLength(500)]
        public string Login { get; set; }

        [Required, StringLength(500)]
        public string Password { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public override string ToString() => $"{Name} {Address}:{Port}, {(EnableSsl? "ssl" : "no ssl")}";
    }
}