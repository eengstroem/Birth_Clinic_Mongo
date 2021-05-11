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
    public interface IClinicianRepository
    {
        // CRUD
        Task<Clinician> Get(int id);
        Task<IEnumerable<Clinician>> GetAll();
        Task<IEnumerable<Clinician>> GetAllMatching(List<int> ids);
        Task<Clinician> Create(Clinician clinician);
        Task<bool> Update(int id, Clinician clinician);
        Task<bool> Delete(int id);

        Task<List<Clinician>> FindAvailableClinicians(ClinicianType role, Birth birth, int RequiredDelta, int AllowedOccurences);
    }
}
