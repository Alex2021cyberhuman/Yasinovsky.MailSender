using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.Core.Data
{
    public interface IMovieDataManager : IDataManager<Movie, Guid>

    {
    }
}
