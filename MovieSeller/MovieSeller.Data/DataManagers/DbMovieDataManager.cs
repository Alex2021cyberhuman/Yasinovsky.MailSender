using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieSeller.Core.Data;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.Data.DataManagers
{
    public class DbMovieDataManager : IMovieDataManager
    {
        private readonly MovieSellerDbContext _context;
        private readonly ILogger<DbMovieDataManager> _logger;

        public DbMovieDataManager(MovieSellerDbContext context, ILogger<DbMovieDataManager> logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> GetWhereAsync(Expression<Func<Movie, bool>> expression)
        {
            return (await _context.Set<Movie>().Where(expression).AsNoTracking().ToListAsync()).AsEnumerable();
        }

        public async Task<Movie> FirstOrDefaultAsync(Expression<Func<Movie, bool>> expression)
        {
            return await _context.Set<Movie>().AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Movie> AddAsync(Movie item)
        {
            var entry = await _context.Set<Movie>().AddAsync(item);
            await _context.SaveChangesAsync();
            _logger?.LogInformation(
                $"{typeof(Movie).FullName} " +
                $"added to {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task<Movie> UpdateAsync(Movie item)
        {
            var entry = _context.Set<Movie>().Update(item);
            await _context.SaveChangesAsync();
            _logger?.LogInformation(
                $"{typeof(Movie).FullName} " +
                $"updated in {_context.GetType().FullName} " +
                $"with values {entry.Entity}");
            return entry.Entity;
        }

        public async Task RemoveAsync(Movie item)
        {
            var entry = _context.Set<Movie>().Remove(item);
            await _context.SaveChangesAsync();
            _logger?.LogInformation(
                $"{typeof(Movie).FullName} " +
                $"removed from {_context.GetType().FullName} " +
                $"with values {entry?.Entity}");
        }
    }
}
