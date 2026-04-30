using PayRoleSystem.Models;

namespace PayRoleSystem.Services.Interfaces
{
    public interface IPagePermissionService
    {
        Task<IEnumerable<RolePagePermission>> GetPagePermissionsByRoleIdAsync(int roleId);
        Task AssignPagePermissionsToRoleAsync(int roleId, IEnumerable<int?> pageIds);
    }

}
