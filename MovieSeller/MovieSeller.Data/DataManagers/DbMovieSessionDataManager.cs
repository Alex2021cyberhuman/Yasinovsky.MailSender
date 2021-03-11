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
    public class DbMovieSessionDataManager : IMovieSessionDataManager
    {
        private readonly MovieSellerDbContext _context;
        private readonly ILogger<DbMovieDataManager> _logger;

        public DbMovieSessionDataManager(MovieSellerDbContext context, ILogger<DbMovieDataManager> logger = null)
        {
            _context = context;
            _logger = logger;
        }


        public IQueryable<MovieSession> Queryable => _context.Set<MovieSession>().AsQueryable();

        public async Task<IEnumerable<MovieSession>> GetWhereAsync(Expression<Func<MovieSession, bool>> expression)
        {
            return (await _context.Set<MovieSession>().Where(expression).AsNoTracking().ToListAsync()).AsEnumerable();
        }

        public async Task<MovieSession> FirstOrDefaultAsync(Expression<Func<MovieSession, bool>> expression)
        {
            return await _context.Set<MovieSession>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<MovieSession> AddAsync(MovieSession item)
        {
            var entry = await _context.Set<MovieSession>().AddAsync(item);
            await _context.SaveChangesAsync();
                                    entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(MovieSession).FullName} " +
                $"added to {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task<MovieSession> UpdateAsync(MovieSession item)
        {
            var entry = _context.Set<MovieSession>().Update(item);
            await _context.SaveChangesAsync();
                        entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(MovieSession).FullName} " +
                $"updated in {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task RemoveAsync(MovieSession item)
        {
            var entry = _context.Set<MovieSession>().Remove(item);

            await _context.SaveChangesAsync();
                        entry.State = EntityState.Detached;
            _context.ChangeTracker.Clear();
            _logger?.LogInformation(
                $"{typeof(MovieSession).FullName} " +
                $"removed from {_context.GetType().FullName} " +
                $"with values {entry?.Entity}");
        }
    }
}