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
    }
}
