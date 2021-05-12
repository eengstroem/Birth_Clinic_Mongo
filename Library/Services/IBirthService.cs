using Library.Models.Births;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Clinicians;

namespace Library.Services
{
    public interface IBirthService
    {
        IEnumerable<Birth> GetAllWithinTimespan(DateTime startDate, DateTime endDate);
        IEnumerable<Birth> GetAllBirthsUsingABirthRoomAtTime(DateTime time);
        IEnumerable<Birth> GetAll();
    }
}
