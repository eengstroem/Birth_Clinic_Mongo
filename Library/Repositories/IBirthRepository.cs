using Library.Models.Births;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Repository
{
    public interface IBirthRepository
    {
        // CRUD
        Task<string> Create(Birth birth);
        Task<Birth> Get(string id);
        IQueryable<Birth> GetAll();
        Task<bool> Update(string id, Birth birth);
        Task<bool> Delete(string id);

    }
}
