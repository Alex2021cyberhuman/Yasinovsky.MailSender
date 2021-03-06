using System;
using MovieSeller.Core.Models.Domain;

namespace MovieSeller.Core.Data
{
    public interface IBookingDataManager : IDataManager<Booking, Guid>

    {
    }
}