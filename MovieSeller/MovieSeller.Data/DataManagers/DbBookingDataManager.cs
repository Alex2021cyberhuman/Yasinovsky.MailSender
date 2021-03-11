using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSeller.Core.Data;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.Data.DataManagers
{
    public class DbBookingDataManager : IBookingDataManager
    {
        private readonly MovieSellerDbContext _context;
        private readonly ILogger<DbMovieDataManager> _logger;

        public DbBookingDataManager(MovieSellerDbContext context, ILogger<DbMovieDataManager> logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<Booking> Queryable => _context.Set<Booking>().AsQueryable();

        public async Task<IEnumerable<Booking>> GetWhereAsync(Expression<Func<Booking, bool>> expression)
        {
            return (await _context.Set<Booking>().Where(expression).AsNoTracking().ToListAsync()).AsEnumerable();
        }

        public async Task<Booking> FirstOrDefaultAsync(Expression<Func<Booking, bool>> expression)
        {
            return await _context.Set<Booking>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Booking> AddAsync(Booking item)
        {
            var entry = await _context.Set<Booking>().AddAsync(item);
            await _context.SaveChangesAsync();
                        entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(Booking).FullName} " +
                $"added to {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task<Booking> UpdateAsync(Booking item)
        {
            var entry = _context.Set<Booking>().Update(item);
            await _context.SaveChangesAsync();
                        entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(Booking).FullName} " +
                $"updated in {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task RemoveAsync(Booking item)
        {
            var entry = _context.Set<Booking>().Remove(item);
            await _context.SaveChangesAsync();
                        entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(Booking).FullName} " +
                $"removed from {_context.GetType().FullName} " +
                $"with values {entry?.Entity}");
        }
    }
}