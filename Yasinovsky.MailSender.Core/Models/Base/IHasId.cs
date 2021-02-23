using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yasinovsky.MailSender.Core.Models.Base
{
    public interface IHasId
    {
        public int Id { get; set; }
    }
}
