using Library.Models.Births;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Clinicians;

namespace Library.Services
{
    interface IBirthService
    {
        Task<(Birth, List<Clinician>)> GetWithClinicians(string id);
        Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate);
        Task<IEnumerable<(Birth, string, IEnumerable<Clinician>)>> GetAllBirthsWithCliniciansUsingBirthRoomAtTime(DateTime time);
        Task<bool> EndBirth(string id);
    }
}
