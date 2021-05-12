using Library.Models.Births;
using System;
using System.Collections.Generic;

namespace Library.Services
{
    public interface IBirthService
    {
        IEnumerable<Birth> GetAllWithinTimespan(DateTime startDate, DateTime endDate);
        IEnumerable<Birth> GetAllBirthsUsingABirthRoomAtTime(DateTime time);
        IEnumerable<Birth> GetAll();
    }
}
