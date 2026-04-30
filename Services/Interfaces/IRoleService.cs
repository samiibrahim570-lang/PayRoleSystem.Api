using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;

namespace PayRoleSystem.Services.Interfaces
{
    public interface IRoleService
    {
        //Task<ResponseModel<bool>> SaveRoleAsync(RoleRequest roleRequest);
        //Task<ResponseModel<IList<RoleResponse>>> SaveRoleAsync(RoleRequest roleRequest);
        //Task<ResponseModel<IList<RoleResponse>>> GetByRoleIdPermissionsAsync(int roleId);
        //Task<ResponseModel<RoleResponse>> GetRoleByTokenAsync();
        //Task<ResponseModel<IList<RolesResponse>>> GetAllRolesAsync();

        //Task<ResponseModel<IList<RoleWithUsersResponse>>> GetAllRolesWithUsersAsync();

        Task<ResponseModel<RoleResponse>> SaveRoleAsync(RoleRequest roleRequest);
        Task<ResponseModel<RoleResponse>> GetByRoleIdPermissionsAsync(int roleId);
        Task<ResponseModel<RoleResponse>> GetRoleByTokenAsync();
        Task<ResponseModel<IList<RoleResponse>>> GetAllRolesAsync();

        Task<ResponseModel<IList<RoleWithUsersResponse>>> GetAllRolesWithUsersAsync();
    }
}
