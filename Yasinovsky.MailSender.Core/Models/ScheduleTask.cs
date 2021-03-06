using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Yasinovsky.MailSender.Core.Models.Base;

namespace Yasinovsky.MailSender.Core.Models
{
    public class ScheduleTask : IHasId
    {
        public int Id { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Scheduled { get; set; }

        [ForeignKey(nameof(Message))]
        public int MessageId { get; set; }

        [JsonIgnore]
        public Message Message { get; set; }

        [ForeignKey(nameof(Sender))]
        public int SenderId { get; set; }

        [JsonIgnore]
        public Sender Sender { get; set; }

        [ForeignKey(nameof(Server))]
        public int ServerId{ get; set; }

        [JsonIgnore]
        public Server Server { get; set; }

        public ICollection<Recipient> Recipients { get; set; }
    }
}
