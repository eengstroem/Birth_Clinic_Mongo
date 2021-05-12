using Library.Models.Births;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class BirthRepository : IBirthRepository
    {

        // Initialization

        private readonly MongoClient _client;
        private readonly IMongoCollection<Birth> _births;
        public BirthRepository(MongoClient client)
        {
            _client = client;
            _client.GetDatabase("BirthClinic").DropCollection(nameof(Birth));
            _births = _client.GetDatabase("BirthClinic")
                .GetCollection<Birth>(nameof(Birth));
        }

        // CRUD

        public async Task<string> Create(Birth birth)
        {
            await _births.InsertOneAsync(birth);
            return birth.Id;
        }

        public async Task<Birth> Get(string id)
        {
            return await _births.Find(b => b.Id == id).FirstAsync();
        }

        public IQueryable<Birth> GetAll()
        {
            return _births.AsQueryable();
        }

        public async Task<bool> Update(string id, Birth birth)
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

        public async Task<bool> Delete(string id)
        {
            var res = await _births.DeleteOneAsync(b => b.Id == id);
            return res.DeletedCount == 1;
        }

        // Specific Task Functionality
        /* public async Task<(Birth, List<Clinician>)> GetWithClinicians(string id)
         {
             var ClinicianList = _client.GetDatabase("BirthClinic").GetCollection<Clinician>(nameof(Clinician));
             var birth = await _births.Find(b => b.Id == id).FirstOrDefaultAsync();
             var clinicians = await ClinicianList.Find(c => c.AssignedBirthsIds.Contains(birth.Id)).ToListAsync();

             return (birth, clinicians);
         }*/

        /* public async Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate)
         {
             return await _births.Find(b => b.BirthDate > startDate & b.BirthDate < endDate).ToListAsync();
         }*/

        /* public async Task<IEnumerable<(Birth, string, IEnumerable<Clinician>)>> GetAllBirthsWithCliniciansUsingBirthRoomAtTime(DateTime time)
         {
             var RoomList = _client.GetDatabase("BirthClinic").GetCollection<Room>(nameof(Room));
             var ClinicianList = _client.GetDatabase("BirthClinic").GetCollection<Clinician>(nameof(Clinician));
             var ValidBirths = await _births.Find(b => b.BirthDate == time).ToListAsync();
             List<(Birth, string, IEnumerable<Clinician>)> result = new();


             foreach (Birth b in ValidBirths)
             {
                 string roomId;
                 foreach (Reservation res in b.Reservations)
                 {
                     var BirthRoom = await RoomList.Find(r => r.RoomType == RoomType.BIRTH & r.Id == res.Room).FirstOrDefaultAsync();
                     if (BirthRoom != null)
                     {
                         roomId = res.Room;
                         IEnumerable<Clinician> clinicians = await ClinicianList.Find(c => c.AssignedBirthsIds.Contains(b.Id)).ToListAsync();
                         result.Append((b, roomId, clinicians));
                     }
                 }


             }
             return result;

         }*/

        public async Task<bool> EndBirth(string id)
        {
            var update = Builders<Birth>.Update
                .Set(b => b.IsEnded, false);

            var res = await _births.UpdateOneAsync(b => b.Id == id, update);
            return res.ModifiedCount == 1;
        }
    }
}
