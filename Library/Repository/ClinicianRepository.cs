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

        public async Task<Clinician> Get(int id)
        {
            return await _clinicians.Find(c => c._id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Clinician>> GetAll()
        {
            return await _clinicians.Find(_ => true).ToListAsync();
        }
        public async Task<IEnumerable<Clinician>> GetAllMatching(List<int> ids)
        {
            return await _clinicians.Find(c => ids.Contains(c._id)).ToListAsync();
        }

        public async Task<bool> Update(int id, Clinician clinician)
        {
            var update = Builders<Clinician>.Update
                .Set(c => c.AssignedBirthsIds, clinician.AssignedBirthsIds)
                .Set(c => c.Role, clinician.Role)
                .Set(c => c.FirstName, clinician.FirstName)
                .Set(c => c.LastName, clinician.LastName);

            var res = await _clinicians.UpdateOneAsync(c => c._id == id, update);
            return res.ModifiedCount == 1;
        }

        public async Task<bool> Delete(int id)
        {
            var res = await _clinicians.DeleteOneAsync(c => c._id == id);
            return res.DeletedCount == 1;
        }

        public async Task<List<Clinician>> FindAvailableClinicians(ClinicianType role,Birth birth, int RequiredDelta, int AllowedOccurences)
        {
            var _births = _client.GetDatabase("BirthClinic").GetCollection<Birth>(nameof(Birth));
            var clinicians = await _clinicians.Find(c =>c.Role == role).ToListAsync();

            var births = await _births.Find(_=>true).ToListAsync();

            List<Clinician> availableClinicians = new();

            foreach(Birth b in births)
            {
                foreach(Clinician c in clinicians)
                {
                    if (!c.AssignedBirthsIds.Contains(b.Id))
                    {
                        break;
                    }
                    if (((birth.BirthDate - b.BirthDate).TotalDays - (birth.BirthDate - b.BirthDate).Days)*60  >= RequiredDelta * 60)
                    {
                        availableClinicians.Add(c);
                    }
                }
            }

            return availableClinicians;
        }
    }
}
