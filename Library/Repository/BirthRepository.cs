using Library.Models.Births;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Models.Clinicians;

namespace Library.Repository
{
    public class BirthRepository : IBirthRepository
    {
        private MongoClient _client;
        private IMongoCollection<Birth> _births;
        public BirthRepository(MongoClient client)
        {
            _client = client;
            _births = _client.GetDatabase("BirthClinic").GetCollection<Birth>(nameof(Birth));
        }

        public async Task<ObjectId> Create(Birth birth)
        {
            await _births.InsertOneAsync(birth);
            return birth.Id;
        }

        public async Task<bool> Delete(ObjectId id)
        {
            var res = await _births.DeleteOneAsync(b => b.Id == id);
            return res.DeletedCount == 1;
        }

        public Task<bool> EndBirth(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async Task<Birth> Get(ObjectId id)
        {
            return await _births.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Birth>> Get()
        {
            return await _births.Find(_ => true).ToListAsync();
        }

        public Task<IEnumerable<Birth>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Birth>> GetAllActiveBirths()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<(Birth, List<Clinician>)> GetWithClinicians(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ObjectId id, Birth birth)
        {
            var update = Builders<Birth>.Update
                .Set(b => b.AssociatedClinicians, birth.AssociatedClinicians)
                .Set(b => b.BirthDate, birth.BirthDate)
                .Set(b => b.ChildrenToBeBorn, birth.ChildrenToBeBorn)
                .Set(b => b.Father, birth.Father)
                .Set(b => b.Mother, birth.Mother)
                .Set(b => b.IsEnded, birth.IsEnded)
                .Set(b => b.Reservations, birth.Reservations)
                .Set(b => b.Relatives, birth.Relatives);

            var res = await _births.UpdateOneAsync(b => b.Id == id, update);
            return res.ModifiedCount == 1;
        }
    }
}
