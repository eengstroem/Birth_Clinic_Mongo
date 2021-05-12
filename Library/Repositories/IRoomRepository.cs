using Library.Models.Rooms;
using System.Linq;
using System.Threading.Tasks;

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
