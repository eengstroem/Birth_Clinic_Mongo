using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Models.Clinicians;
using Library.Models.Births;
using MongoDB.Bson;

namespace Library.Repository
{
    public class ClinicianRepository : IClinicianRepository
    {
        // Initialization

        private MongoClient _client;
        private IMongoCollection<Clinician> _clinicians;

        public ClinicianRepository(MongoClient client)
        {
            _client = client;
            _clinicians = _client.GetDatabase("BirthClinic")
                .GetCollection<Clinician>(nameof(Clinician));
        }

        // CRUD

        public async Task<int> Create(Clinician clinician)
        {
            await _clinicians.InsertOneAsync(clinician);
            return clinician.Id;
        }

        public async Task<Clinician> Get(int id)
        {
            return await _clinicians.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Clinician>> GetAll()
        {
            return await _clinicians.Find(_ => true).ToListAsync();
        }
        public Task<IEnumerable<Clinician>> GetAllMatching(List<string> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(int id, Clinician clinician)
        {
            var update = Builders<Clinician>.Update
                .Set(c => c.AssignedBirthsIds, clinician.AssignedBirthsIds)
                .Set(c => c.Role, clinician.Role)
                .Set(c => c.FirstName, clinician.FirstName)
                .Set(c => c.LastName, clinician.LastName);

            var res = await _clinicians.UpdateOneAsync(c => c.Id == id, update);
            return res.ModifiedCount == 1;
        }

        public async Task<bool> Delete(int id)
        {
            var res = await _clinicians.DeleteOneAsync(c => c.Id == id);
            return res.DeletedCount == 1;
        }
    }
}
