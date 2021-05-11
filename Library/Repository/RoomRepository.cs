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
            _rooms = _client.GetDatabase("BirthClinic")
                .GetCollection<Room>(nameof(Room));
        }
        // CRUD
        public async Task<int> Create(Room room)
        {
            await _rooms.InsertOneAsync(room);
            return room.Id;
        }

        public async Task<Room> Get(int id)
        {
            return await _rooms.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Room>> GetAll()
        {
            return await _rooms.Find(_ => true).ToListAsync();
        }

        public async Task<bool> Update(int id, Room room)
        {
            var update = Builders<Room>.Update
                .Set(r => r.RoomType, room.RoomType);

            var res = await _rooms.UpdateOneAsync(r => r.Id == id, update);
            return res.ModifiedCount == 1;
        }
        public async Task<bool> Delete(int id)
        {
            var res = await _rooms.DeleteOneAsync(r => r.Id == id);
            return res.DeletedCount == 1;
        }
        // Specific Task Functionality
        public async Task<IEnumerable<Room>> GetAllMatching(List<int> ids)
        {
            return await _rooms.Find(r => ids.Contains(r.Id)).ToListAsync();
        }


        // ??? I don't get, vi har kun ID og RoomType, så return room og int, why not just get rooms og call .Id?
        public Task<Dictionary<int, Room>> GetDictionaryOfAllMatching(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Room> GetUnreservedRoomOfType(DateTime StartTime, DateTime EndTime, RoomType Type)
        {
            var _births = _client.GetDatabase("BirthClinic").GetCollection<Birth>(nameof(Birth));
            var rooms = await _rooms.Find(r => r.RoomType == Type).ToListAsync();
            var births = await _births.Find(_ => true).ToListAsync();

            if(births == null)
            {
                return rooms.ElementAt(0);
            }
            foreach (Room r in rooms)
            {
                foreach(Birth b in births)
                {
                    foreach(Reservation res in b.Reservations)
                    {
                        if(r.ReservationIds.Contains(res.Id))
                        {
                            if(res.EndTime < StartTime && res.StartTime < StartTime || res.StartTime > EndTime && res.EndTime > EndTime)
                            {
                                return r;
                            }
                        }
                    }
                }
            }
            return null;
        }

    }
}
