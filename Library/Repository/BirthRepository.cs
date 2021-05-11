﻿using Library.Models.Births;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using Library.Models.Clinicians;
using Library.Models.Reservations;
using Library.Models.Rooms;

namespace Library.Repository
{
    public class BirthRepository : IBirthRepository
    {

        // Initialization

        private MongoClient _client;
        private IMongoCollection<Birth> _births;
        public BirthRepository(MongoClient client)
        {
            _client = client;
            _births = _client.GetDatabase("BirthClinic").GetCollection<Birth>(nameof(Birth));
        }

        // CRUD

        public async Task<int> Create(Birth birth)
        {
            await _births.InsertOneAsync(birth);
            return birth.Id;
        }

        public async Task<Birth> Get(int id)
        {
            return await _births.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Birth>> GetAll()
        {
            return await _births.Find(_ => true).ToListAsync();
        }

        public async Task<bool> Update(int id, Birth birth)
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

        public async Task<bool> Delete(int id)
        {
            var res = await _births.DeleteOneAsync(b => b.Id == id);
            return res.DeletedCount == 1;
        }

        // Specific Task Functionality
        public async Task<(Birth, List<Clinician>)> GetWithClinicians(int id)
        {
            var ClinicianList = _client.GetDatabase("BirthClinic").GetCollection<Clinician>(nameof(Clinician));
            Birth birth = await _births.Find(b => b.Id == id).FirstOrDefaultAsync();
            List<Clinician> clinicians = await ClinicianList.Find(c => c.AssignedBirthsIds.Contains(birth.Id)).ToListAsync();

            return (birth, clinicians);
        }

        public async Task<IEnumerable<Birth>> GetAllWithinTimespan(DateTime startDate, DateTime endDate)
        {
            return await _births.Find(b => b.BirthDate > startDate & b.BirthDate < endDate).ToListAsync();
        }

        public async Task<IEnumerable<(Birth, int, IEnumerable<Clinician>)>> GetAllBirthsWithCliniciansUsingBirthRoomAtTime(DateTime time)
        {
            var RoomList = _client.GetDatabase("BirthClinic").GetCollection<Room>(nameof(Room));
            var ClinicianList = _client.GetDatabase("BirthClinic").GetCollection<Clinician>(nameof(Clinician));
            var ValidBirths = await _births.Find(b => b.BirthDate == time).ToListAsync();
            IEnumerable<(Birth, int, IEnumerable<Clinician>)> result = null;


            foreach (Birth b in ValidBirths)
            {
                int room = 0;
                foreach (Reservation res in b.Reservations)
                {
                    var BirthRoom = await RoomList.Find(r => r.RoomType == RoomType.BIRTH & r.Id == res.ReservedRoomId).FirstOrDefaultAsync();
                    if (BirthRoom != null)
                    {
                        room = res.ReservedRoomId;
                    }
                }
                IEnumerable<Clinician> clinicians = await ClinicianList.Find(c => c.AssignedBirthsIds.Contains(b.Id)).ToListAsync();
                result = result.Append((b, room, clinicians));
            }
            return result;

        }

        public async Task<bool> EndBirth(int id)
        {
            var update = Builders<Birth>.Update
                .Set(b => b.IsEnded, false);

            var res = await _births.UpdateOneAsync(b => b.Id == id, update);
            return res.ModifiedCount == 1;
        }
    }
}
