using Library.Models.Births;
using Library.Models.Reservations;
using Library.Models.Rooms;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repository
{
    public class RoomRepository : IRoomRepository
    {
        // Initialization

        private readonly MongoClient _client;
        private readonly IMongoCollection<Room> _rooms;

        public RoomRepository(MongoClient client)
        {
            _client = client;

            _client.GetDatabase("BirthClinic").DropCollection(nameof(Room));
            _rooms = _client.GetDatabase("BirthClinic")
                .GetCollection<Room>(nameof(Room));
        }
        // CRUD
        public async Task<Room> Create(Room room)
        {
            await _rooms.InsertOneAsync(room);
            
            return room;
        }

        public async Task<Room> Get(string id)
        {
            return await _rooms.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Room> GetAll()
        {
            return  _rooms.AsQueryable();
        }

        public async Task<bool> Update(string id, Room room)
        {
            var update = Builders<Room>.Update
                .Set(r => r.RoomType, room.RoomType);

            var res = await _rooms.UpdateOneAsync(r => r.Id == id, update);
            return res.ModifiedCount == 1;
        }
        public async Task<bool> Delete(string id)
        {
            var res = await _rooms.DeleteOneAsync(r => r.Id == id);
            return res.DeletedCount == 1;
        }

        // Specific Task Functionality
        public async Task<IEnumerable<Room>> GetAllMatching(List<string> ids)
        {
            return await _rooms.Find(r => ids.Contains(r.Id)).ToListAsync();
        }


        // ??? I don't get, vi har kun ID og RoomType, så return room og int, why not just get rooms og call .Id?
        public Task<Dictionary<string, Room>> GetDictionaryOfAllMatching(List<string> ids)
        {
            throw new NotImplementedException();
        }
        
    }
}
