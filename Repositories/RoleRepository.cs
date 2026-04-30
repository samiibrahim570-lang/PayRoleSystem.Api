using PayRoleSystem.Data;
using PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PayRoleSystem.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.Name == name);
        }


        public async Task<Role> GetListByIdAsync(int id)
        {
            return await _context.role
                .Include(r => r.RolePagePermissions)
                    .ThenInclude(rpp => rpp.Page)
                //.ThenInclude(rpp => rpp.RoleControlPermissions) // Uncomment if needed
                .FirstOrDefaultAsync(x => x.Id == id);
        }




    }
}
