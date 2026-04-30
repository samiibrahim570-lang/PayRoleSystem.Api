using PayRoleSystem.Http;
using PayRoleSystem.Models;
using PayRoleSystem.Request;
using PayRoleSystem.Response;


namespace PayRoleSystem.Services
{
    public interface IAuthService
    {

        Task<AuthResponseDto> Authenticate(AuthRequestDto request);
        //Task<ResponseModel<ApplicationUser>> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}
