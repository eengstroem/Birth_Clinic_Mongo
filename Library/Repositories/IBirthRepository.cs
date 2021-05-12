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
        Task<string> Create(Birth birth);
        Task<Birth> Get(string id);
        IQueryable<Birth> GetAll();
        Task<bool> Update(string id, Birth birth);
        Task<bool> Delete(string id);

    }
}
