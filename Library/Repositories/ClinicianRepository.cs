using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Models.Clinicians;
using Library.Models.Births;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Library.Repository
{
    public class ClinicianRepository : IClinicianRepository
    {
        // Initialization

        private readonly MongoClient _client;
        private readonly IMongoCollection<Clinician> _clinicians;

        public ClinicianRepository(MongoClient client)
        {
            _client = client;
            _clinicians = _client.GetDatabase("BirthClinic")
                .GetCollection<Clinician>(nameof(Clinician));
        }

        // CRUD

        public async Task<Clinician> Create(Clinician clinician)
        {
            await _clinicians.InsertOneAsync(clinician);
            return clinician;
        }

        public async Task<Clinician> Get(string id)
        {
            return await _clinicians.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Clinician> GetAll()
        {
            return _clinicians.AsQueryable();
        }
        public IQueryable<Clinician> GetAllMatching(List<string> ids)
        {
            var filter = Builders<Clinician>.Filter.In(c => c.Id, ids);
            return _clinicians.AsQueryable().Where(c => filter.Inject());
        }

        public async Task<bool> Update(string id, Clinician clinician)
        {
            var update = Builders<Clinician>.Update
                .Set(c => c.AssignedBirthsIds, clinician.AssignedBirthsIds)
                .Set(c => c.Role, clinician.Role)
                .Set(c => c.FirstName, clinician.FirstName)
                .Set(c => c.LastName, clinician.LastName);

            var res = await _clinicians.UpdateOneAsync(c => c.Id == id, update);
            return res.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string id)
        {
            var res = await _clinicians.DeleteOneAsync(c => c.Id == id);
            return res.DeletedCount == 1;
        }

 
    }
}
