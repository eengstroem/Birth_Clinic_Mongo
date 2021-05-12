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
        Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Birth>>  GetAllBirthsUsingABirthRoomAtTime(DateTime time);
        Task<IEnumerable<Birth>> GetAll();
    }
}
