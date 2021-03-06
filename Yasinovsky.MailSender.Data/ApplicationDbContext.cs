using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yasinovsky.MailSender.Core.Models;

namespace Yasinovsky.MailSender.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Recipient> Recipients { get; set; }

        public DbSet<ScheduleTask> ScheduleTasks { get; set; }

        public DbSet<Sender> Senders { get; set; }

        public DbSet<Server> Servers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ScheduleTask>(entity =>
            {
                entity.HasMany(x => x.Recipients)
                    .WithMany(x => x.ScheduleTasks);
            });
        }
    }
}
