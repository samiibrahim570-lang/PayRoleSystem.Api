using PayRoleSystem.Data;
using PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PayRoleSystem.Repositories
{
    public class RolePagePermissionRepository : GenericRepository<RolePagePermission>, IRolePagePermissionRepository
    {
        public RolePagePermissionRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        // Fetch role page permissions by roleId
        public async Task<List<RolePagePermission>> GetByRoleIdAsync(int roleId)
        {
            return await _context.rolePagePermission
                .Where(rpp => rpp.RoleId == roleId)
                .ToListAsync();
        }



    }
}