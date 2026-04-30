
using PayRoleSystem.Api.DTOs.Request;
using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;
using PayRoleSystem.Models;


namespace PayRoleSystem.Services
{
    public interface IApplicationUserService
    {

        Task<ResponseModel<IEnumerable<ApplicationUserResponse>>> GetAllUsersAsync();
        Task<ResponseModel<ApplicationUserResponse>> GetUserByGlobalIdAsync(Guid GlobalId);
        Task<ResponseModel<ApplicationUserResponse>> GetUserByTokenAsync();
        Task<ResponseModel<ApplicationUserResponse>> RegisterUserAsync(ApplicationUserRequestDto request);
        Task<ResponseModel<string>> ForgotPasswordAsync(ForgotPasswordRequestDto request);
        Task<ResponseModel<ApplicationUserResponse>> UpdateUserAsync(Guid globalId, ApplicationUserRequestDto request);
        Task<ResponseModel<ApplicationUserResponse>> DeleteUserAsync(Guid globalId);
        Task<ResponseModel<PageResult<ApplicationUserResponse>>> Search(SearchRequest searchRequest);
        Task<ResponseModel<string>> UploadUserImageAsync(int UserId, IFormFile imageFile);

        Task<ResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
