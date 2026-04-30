using PayRoleSystem.Data;
using PayRoleSystem.Request;
using PayRoleSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace PayRoleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }
          
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
        {
            if (request == null ||
                (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.ContactNumber)) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Please provide an email or contact number and a password.");
            }

            try
            {
                var response = await _authService.Authenticate(request);
                if (string.IsNullOrEmpty(response.Token))
                {
                    return Unauthorized("Authentication failed. Token not generated.");
                }

                return Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
