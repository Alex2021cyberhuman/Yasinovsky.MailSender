using System.ComponentModel.DataAnnotations.Schema;

namespace Yasinovsky.MailSender.Core.Models
{
    [Table("Sender")]
    public sealed class Sender : EmailAddressInfo { }
}