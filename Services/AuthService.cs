
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PayRoleSystem.Data;
using PayRoleSystem.Models;
using PayRoleSystem.Response;
using PayRoleSystem.Request;
using PayRoleSystem.Http;
using PayRoleSystem.Repository;

namespace PayRoleSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IApplicationUserRepository _applicationUserRepository;



        public AuthService(IConfiguration configuration, ApplicationDbContext context, IApplicationUserRepository applicationUserRepository)
        {
            _configuration = configuration;
            _context = context;
            _applicationUserRepository = applicationUserRepository;
        }

        
        public string GenerateJwtToken(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");
            var issuer = jwtSettings.GetValue<string>("Issuer");
            var audience = jwtSettings.GetValue<string>("Audience");

            if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT settings are not configured properly.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            //new Claim(ClaimTypes.Name, user.UserName
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            // Add more claims as necessary
        };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public ApplicationUser ValidateUser(string email, string password)
        //{
        //    var user = _context.applicationUsers.SingleOrDefault(u => u.Email == email);
        //    if (user == null || user.Password != password)
        //        return null;

        //    return user;
        //}


        //public async Task<AuthResponseDto> Authenticate(AuthRequestDto request)
        //{
        //    if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.ContactNumber))
        //    {
        //        throw new UnauthorizedAccessException("Either email or contact number must be provided.");
        //    }

        //    var user = await _context.applicationUser
        //        .Where(u =>
        //            (!string.IsNullOrEmpty(request.Email) && u.Email == request.Email) ||
        //            (!string.IsNullOrEmpty(request.ContactNumber) && u.ContactNumber == request.ContactNumber))
        //        .FirstOrDefaultAsync();

        //    if (user == null || user.Password != request.Password)
        //    {
        //        throw new UnauthorizedAccessException("Invalid credentials");
        //    }

        //    var token = GenerateJwtToken(user);

        //    return new AuthResponseDto
        //    {
        //        Token = token
        //    };
        //}

        public async Task<AuthResponseDto> Authenticate(AuthRequestDto request)

        {
            if (string.IsNullOrEmpty(request.Email))
            {
                throw new UnauthorizedAccessException("Either email or contact number must be provided.");
            }

            var user = await _context.applicationUser
                .Where(u =>
                    (!string.IsNullOrEmpty(request.Email) && (u.Email == request.Email || u.ContactNumber == request.Email))
                )
                .FirstOrDefaultAsync();


            if (user == null || user.Password != request.Password)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    UserId = user.Id,
                    RoleId = user.RoleId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    ContactNumber = user.ContactNumber,
                    WhatsAppNumber = user.WhatsAppNumber,
                    PhoneNumber = user.PhoneNumber,
                    Image = user.Image
                }
            };

        }



        private bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new System.Net.Mail.MailAddress(email);
                return emailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }


    }
}
