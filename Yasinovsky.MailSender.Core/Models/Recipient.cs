using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yasinovsky.MailSender.Core.Models
{
    [Table("Recipient")]
    public sealed class Recipient : EmailAddressInfo
    {
        public ICollection<ScheduleTask> ScheduleTasks { get; set; }
    }
}