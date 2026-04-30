

using PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;

namespace PayRoleSystem.Repository
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<ApplicationUser> GetByUsernameAsync(string emailAddress);
        Task<IList<ApplicationUser>> GetListByRoleIdAsync(int roleId);


        //Task<PageResult<ApplicationUser>> GetFilteredUsersAsync(UserFilterDto filterDto);

    }
}
