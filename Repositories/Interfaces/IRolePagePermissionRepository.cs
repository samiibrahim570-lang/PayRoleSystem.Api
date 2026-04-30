using PayRoleSystem.Models;

namespace PayRoleSystem.Repositories.Interfaces
{
    public interface IRolePagePermissionRepository : IGenericRepository<RolePagePermission>
    {
        Task<List<RolePagePermission>> GetByRoleIdAsync(int roleId);
    }
}
