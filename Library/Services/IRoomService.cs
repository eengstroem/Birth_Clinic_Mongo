using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models.Rooms;

namespace Library.Services
{
    interface IRoomService
    {
        Task<IEnumerable<Room>> GetAllMatching(List<string> ids);
        Task<Dictionary<string, Room>> GetDictionaryOfAllMatching(List<string> ids);
        Task<Room> GetUnreservedRoomOfType(DateTime StartTime, DateTime EndTime, RoomType Type);
    }
}
