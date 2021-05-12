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
        Task<Clinician> Get(string id);
        IQueryable<Clinician> GetAll();
        IQueryable<Clinician> GetAllMatching(List<string> ids);
        Task<Clinician> Create(Clinician clinician);
        Task<bool> Update(string id, Clinician clinician);
        Task<bool> Delete(string id);

       
    }
}
