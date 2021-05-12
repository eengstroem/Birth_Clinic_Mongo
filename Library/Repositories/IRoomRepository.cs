using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Rooms;
using MongoDB.Bson;

namespace Library.Repository
{
    public interface IRoomRepository
    {
        // CRUD
        Task<Room> Get(string id);
        IQueryable<Room> GetAll();
        Task<Room> Create(Room clinician);
        Task<bool> Update(string id, Room clinician);
        Task<bool> Delete(string id);
    }
}
