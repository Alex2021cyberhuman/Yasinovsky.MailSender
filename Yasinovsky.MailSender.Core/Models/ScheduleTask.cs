using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Models
{
    public class ScheduleTask
    {
        public int Id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Scheduled { get; set; }

        [ForeignKey(nameof(Message))]
        public int MessageId { get; set; }

        public Message Message { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        public Sender Sender { get; set; }

        [ForeignKey(nameof(Server))]
        public int ServerId{ get; set; }

        public Server Server { get; set; }

        public ICollection<Recipient> Recipients { get; set; }
    }
}
