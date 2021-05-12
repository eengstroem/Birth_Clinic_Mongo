using Library.Models.Clinicians;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IClinicianRepository
    {
        // CRUD
        Task<Clinician> Get(string id);
        IQueryable<Clinician> GetAll();
        IQueryable<Clinician> GetAllMatching(List<string> ids);
        Task<Clinician> Create(Clinician clinician);
        Task<bool> Update(string id, Clinician clinician);
        Task<bool> Delete(string id);


    }
}
