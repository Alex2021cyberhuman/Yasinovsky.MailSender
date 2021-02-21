using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Core.Contracts.Services
{
    public interface IServerUserDialogService
    {
        public Task<Server> OpenCreateDialogAsync();
        public Task<Server> OpenEditDialogAsync(Server server);
    }
}
