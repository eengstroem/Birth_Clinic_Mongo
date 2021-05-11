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

        Task<IEnumerable<Room>> GetAllMatching(List<int> ids);
        Task<Dictionary<int,Room>> GetDictionaryOfAllMatching(List<int> ids);
    }
}
