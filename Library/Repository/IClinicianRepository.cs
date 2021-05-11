using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Clinicians;
using MongoDB.Bson;

namespace Library.Repository
{
    public interface IClinicianRepository
    {
        Task<IEnumerable<Clinician>> GetAllMatching(List<string> ids);

    }
}
