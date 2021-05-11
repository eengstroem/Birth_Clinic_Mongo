using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Births;
using Library.Models.Clinicians;
using MongoDB.Bson;

namespace Library.Repository
{
    public interface IBirthRepository
    {
        // CRUD
        Task<int> Create(Birth birth);
        Task<Birth> Get(int id);
        Task<IEnumerable<Birth>> GetAll();
        Task<bool> Update(int id, Birth birth);
        Task<bool> Delete(int id);

        // Specific Task Functionality
        Task<(Birth, List<Clinician>)> GetWithClinicians(int id);
        Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate);
        Task<IEnumerable<(Birth, int, IEnumerable<Clinician>)>> GetAllBirthsWithCliniciansUsingBirthRoomAtTime(DateTime time);
        Task<bool> EndBirth(int id);
    }
}
