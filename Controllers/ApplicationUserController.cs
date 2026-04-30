using PayRoleSystem.Data;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;
using PayRoleSystem.Services;
using Microsoft.AspNetCore.Mvc;
using PayRoleSystem.DTOs.Request;
using PayRoleSystem.Api.DTOs.Request;

namespace PayRoleSystem.Controllers
{
    public class ApplicationUserController : BaseApiController
    { 
        private readonly IAuthService _authService; 
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUserService _userService;

        public ApplicationUserController(IAuthService authService, ApplicationDbContext context, IApplicationUserService applicationUserService)
        {
            _authService = authService;
            _context = context;
            _userService = applicationUserService;
        }


        [HttpGet("getAll")]
        public async Task<ResponseModel<IEnumerable<ApplicationUserResponse>>> GetAll()
        {
            var response = await _userService.GetAllUsersAsync();
            return response;
        }

        
        [HttpGet("getBy-GlobalId")]
        public async Task<ResponseModel<ApplicationUserResponse>> GetByGlobalId([FromQuery] Guid GlobalId)
        {
            var result = await _userService.GetUserByGlobalIdAsync(GlobalId);
            return result;
        }


        [HttpGet("getByToken")]
        public async Task<ResponseModel<ApplicationUserResponse>> GetByToken()
        {
            var result = await _userService.GetUserByTokenAsync();
            return result;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseModel<ApplicationUserResponse>>> Register([FromBody] ApplicationUserRequestDto request)
        {
            var response = await _userService.RegisterUserAsync(request);

            if (response.HttpStatusCode == 200)
            {
                return Ok(response);
            }
            else if (response.HttpStatusCode == 400)
            {
                return BadRequest(response);
            }
            else if (response.HttpStatusCode == 500)
            {
                return StatusCode(500, response);
            }

            return StatusCode(500, new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "An unexpected error occurred.",
                HttpStatusCode = 500
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var response = await _userService.ForgotPasswordAsync(request);
            return Ok(response);
        }

        [HttpPut("updateBy-GlobalId")]
        public async Task<ResponseModel<ApplicationUserResponse>> Update([FromQuery] Guid globalId, [FromBody] ApplicationUserRequestDto request)
        {
            var response = await _userService.UpdateUserAsync(globalId, request);
            return response;
        }

        [HttpDelete("deleteBy-GlobalId")]
        public async Task<ResponseModel<ApplicationUserResponse>> Delete([FromQuery] Guid globalId)
        {
            var response = await _userService.DeleteUserAsync(globalId);
            return response;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchRequest searchRequest)
        {
            if (searchRequest == null)
            {
                return BadRequest("Search request cannot be null.");
            }

            var result = await _userService.Search(searchRequest);
            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadUserImageAsync(int UserId, IFormFile imageFile)
        {
            var response = await _userService.UploadUserImageAsync(UserId, imageFile);

            if (response.HttpStatusCode == 200)
            {
                return Ok(response);
            }

            return StatusCode(response.HttpStatusCode, response);
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (request == null)
                return BadRequest(new { Message = "Request cannot be null" });
            var response = await _userService.ResetPasswordAsync(request);
            if (response.HttpStatusCode == 400)
                return BadRequest(response);
            if (response.HttpStatusCode == 404)
                return NotFound(response);
            return Ok(response);
        }



    }
}
