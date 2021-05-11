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
        Task<Room> Get(int id);
        Task<IEnumerable<Room>> GetAll();
        Task<int> Create(Room clinician);
        Task<bool> Update(int id, Room clinician);
        Task<bool> Delete(int id);

        // Specific Task Functionality
        Task<IEnumerable<Room>> GetAllMatching(List<int> ids);
        Task<Dictionary<int, Room>> GetDictionaryOfAllMatching(List<int> ids);
    }
}
