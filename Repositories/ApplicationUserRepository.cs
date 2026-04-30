
using PayRoleSystem.Data;
using PayRoleSystem.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PayRoleSystem.Repository
{
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        //public ApplicationUserRepository(ApplicationDbContext context) : base(context) { }
        public ApplicationUserRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }




        public async Task<ApplicationUser> GetByUsernameAsync(string emailAddress)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == emailAddress && !u.IsDeleted);
        }
        public async Task<IList<ApplicationUser>> GetListByRoleIdAsync(int roleId)
        {
            return await _context.applicationUser
                                 .Where(u => u.RoleId == roleId)
                                 .ToListAsync();
        }

    }
}
