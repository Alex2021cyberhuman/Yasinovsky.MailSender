using System;
using Microsoft.EntityFrameworkCore;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.Data
{
    public class MovieSellerDbContext : DbContext
    {
        protected MovieSellerDbContext()
        {
        }

        public MovieSellerDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<MovieSession> MovieSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieSession>(entity =>
            {
                entity.HasNoDiscriminator();
                entity.Navigation(x => x.Movie)
                    .AutoInclude()
                    .IsRequired();
                entity.Navigation(x => x.Bookings)
                    .AutoInclude();
            });
            modelBuilder.Entity<Booking>()
                .HasNoDiscriminator();
            modelBuilder.Entity<Movie>()
                .HasNoDiscriminator();
        }
    }
}
