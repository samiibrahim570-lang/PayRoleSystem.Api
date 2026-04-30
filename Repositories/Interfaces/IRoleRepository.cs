using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Models;

namespace PayRoleSystem.Repositories.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);

        Task<Role> GetListByIdAsync(int id);

    }
}
