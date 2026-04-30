using PayRoleSystem.Data;
using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;
using PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Services;
using PayRoleSystem.Services.Interfaces;
using PayRoleSystem.UOW;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using PayRoleSystem.Api.DTOs.Request;
using PayRoleSystem.Api.Services.Interfaces;

public class ApplicationUserService : IApplicationUserService
{
    private readonly IUnitOfWork _unitOfWork; 
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IMinioService _minioService;
    private readonly ApplicationDbContext _context;
    private readonly IMinioService _minioService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationUserService(
        IHttpContextAccessor httpContextAccessor,
        IMinioService minioService,
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IEmailService emailService,
        ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork; 
        _mapper = mapper;
        _emailService = emailService;
        _context = context;
        _minioService = minioService;
        _httpContextAccessor = httpContextAccessor;
    }

    //GetAll
    public async Task<ResponseModel<IEnumerable<ApplicationUserResponse>>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.ApplicationUserRepository.GetAllAsync();
        var response = _mapper.Map<IEnumerable<ApplicationUserResponse>>(users);

        // Add role names to each user response
        foreach (var user in response)
        {
            if (user.RoleId.HasValue) // Assuming RoleId is nullable
            {
                user.RoleName = await _getRoleNameById(user.RoleId.Value); 
                user.CreatedByName = await _getCreatedPersonById(user.CreatedBy); 
            }
        }

        return new ResponseModel<IEnumerable<ApplicationUserResponse>>
        {
            Result = response,
            Message = "Users retrieved successfully",
            HttpStatusCode = 200
        };
    }

    private async Task<string> _getRoleNameById(int roleId)
    {
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
        return role?.Name ?? "";
    }

    private async Task<string> _getCreatedPersonById(long createdBy)
    {
        var createdPerson = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(createdBy);
        return (createdPerson?.FirstName + " " + createdPerson?.LastName)?.Trim() ?? ""; 
    }


    //GetBy-Token
    public async Task<ResponseModel<ApplicationUserResponse>> GetUserByTokenAsync()
    {
        // Extract the token from the Authorization header (bearer token)
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "Token is missing in the request header",
                HttpStatusCode = 400
            };
        }

        var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found in token",
                HttpStatusCode = 400
            };
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "Invalid user ID in token",
                HttpStatusCode = 400
            };
        }

        var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found",
                HttpStatusCode = 404
            };
        }

        var role = await _context.role.FirstOrDefaultAsync(r => r.Id == user.RoleId);
        if (role == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "Role not found",
                HttpStatusCode = 404
            };
        }

        var response = _mapper.Map<ApplicationUserResponse>(user);

        response.RoleId = user.RoleId;  
        response.RoleName = role.Name;  

        return new ResponseModel<ApplicationUserResponse>
        {
            Result = response,
            Message = "User retrieved successfully",
            HttpStatusCode = 200
        };
    }

    //GetBy-Id
    public async Task<ResponseModel<ApplicationUserResponse>> GetUserByIdAsync(int id)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(id);
        if (user == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found",
                HttpStatusCode = 404
            };
        }

        var response = _mapper.Map<ApplicationUserResponse>(user);
        return new ResponseModel<ApplicationUserResponse>
        {
            Result = response,
            Message = "User retrieved successfully",
            HttpStatusCode = 200
        };
    }

    //GetBy-GlobalId
    public async Task<ResponseModel<ApplicationUserResponse>> GetUserByGlobalIdAsync(Guid GlobalId)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByGlobalIdAsync(GlobalId);
        if (user == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found",
                HttpStatusCode = 404
            };
        }

        var response = _mapper.Map<ApplicationUserResponse>(user);
        return new ResponseModel<ApplicationUserResponse>
        {
            Result = response,
            Message = "User retrieved successfully",
            HttpStatusCode = 200
        };
    }

    //Register User
    public async Task<ResponseModel<ApplicationUserResponse>> RegisterUserAsync(ApplicationUserRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.ContactNumber))
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "Email or contact number is required.",
                HttpStatusCode = 400
            };
        }

        // Scenario 1: If email is provided
        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingUserByEmail = await _unitOfWork.ApplicationUserRepository.FindAsync(u => u.Email == request.Email && u.IsDeleted == false);
            if (existingUserByEmail.Any())
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = "Email already exists. Please use a different email.",
                    HttpStatusCode = 400
                };
            }

            if (!string.IsNullOrEmpty(request.ContactNumber))
            {
                var existingUserByContact = await _unitOfWork.ApplicationUserRepository.FindAsync(u => u.ContactNumber == request.ContactNumber && u.IsDeleted == false);
                if (existingUserByContact.Any())
                {
                    return new ResponseModel<ApplicationUserResponse>
                    {
                        Result = null,
                        Message = "Contact number already exists. Please use a different contact number.",
                        HttpStatusCode = 400
                    };
                }
            }


            // Check if password and confirm password are provided
            if (!string.IsNullOrEmpty(request.Password) && !string.IsNullOrEmpty(request.ConfirmPassword))
            {
                var passwordValidationResult = ValidatePassword(request.Password, request.ConfirmPassword);
                if (!string.IsNullOrEmpty(passwordValidationResult))
                {
                    return new ResponseModel<ApplicationUserResponse>
                    {
                        Result = null,
                        Message = passwordValidationResult,
                        HttpStatusCode = 400
                    };
                }

                // Map and save user directly
                var user = _mapper.Map<ApplicationUser>(request);
                await _unitOfWork.ApplicationUserRepository.AddAsync(user);

                var response = _mapper.Map<ApplicationUserResponse>(user);

                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = response,
                    Message = "User registered successfully with password.",
                    HttpStatusCode = 200
                };
            }
            else
            {
                // If password is not provided, send an email to create the password
                var user = _mapper.Map<ApplicationUser>(request);

                string subject = "Create a New Password";
                string body = $"Hello {request.FirstName},<br><br>" +
                              "Please click the link below to create your password:<br>" +
                              $"<a href='https://fms.persistentsolutions.co/auth/create-password\r\n?globalId={user.GlobalId}'>Create Password</a><br><br>" +
                              "Thank you!";

                var emailSent = await _emailService.SendEmailAsync(request.Email, subject, body);
                if (!emailSent)
                {
                    return new ResponseModel<ApplicationUserResponse>
                    {
                        Result = null,
                        Message = "Failed to send email.",
                        HttpStatusCode = 500
                    };
                }

                await _unitOfWork.ApplicationUserRepository.AddAsync(user);

                var response = _mapper.Map<ApplicationUserResponse>(user);

                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = response,
                    Message = "User registered successfully. Email sent for password creation.",
                    HttpStatusCode = 200
                };
            }
        }

        // Scenario 2: If only contact number is provided
        if (!string.IsNullOrEmpty(request.ContactNumber))
        {
            var existingUserByContact = await _unitOfWork.ApplicationUserRepository.FindAsync(u => u.ContactNumber == request.ContactNumber);
            if (existingUserByContact.Any()) 
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = "Contact number already exists. Please use a different contact number.",
                    HttpStatusCode = 400
                };
            }

            var passwordValidationResult = ValidatePassword(request.Password, request.ConfirmPassword);
            if (!string.IsNullOrEmpty(passwordValidationResult))
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = passwordValidationResult,
                    HttpStatusCode = 400
                };
            }

            var userToRegister = _mapper.Map<ApplicationUser>(request);

            await _unitOfWork.ApplicationUserRepository.AddAsync(userToRegister);

            var response = _mapper.Map<ApplicationUserResponse>(userToRegister);

            return new ResponseModel<ApplicationUserResponse>
            {
                Result = response,
                Message = "User registered successfully with contact number.",
                HttpStatusCode = 200
            };
        }

        return new ResponseModel<ApplicationUserResponse>
        {
            Result = null,
            Message = "Email or contact number is required.",
            HttpStatusCode = 400
        };
    }

    //uplaod
    public async Task<ResponseModel<string>> UploadUserImageAsync(int UserId, IFormFile imageFile)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("File stream is empty or null.");
            }

            var user = (await _unitOfWork.ApplicationUserRepository.FindAsync(c => c.Id == UserId)).FirstOrDefault();

            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Message = "User not found.",
                    HttpStatusCode = 404
                };
            }

            var uploadResult = await _minioService.UploadFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType, "ApplicationUsers");

            if (uploadResult.HttpStatusCode != 200)
            {
                return new ResponseModel<string>
                {
                    Message = "Error occurred while uploading the image.",
                    HttpStatusCode = 500
                };
            }

            user.Image = uploadResult.Result;
            await _unitOfWork.ApplicationUserRepository.UpdateAsync(user);

            return new ResponseModel<string>
            {
                Message = "Image uploaded successfully.",
                Result = uploadResult.Result,
                HttpStatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel<string>
            {
                Message = "Error occurred while uploading the file.",
                Result = null,
                HttpStatusCode = 500,
                Errors = new List<string> { ex.Message }
            };
        }
        }
    }
    //Forgot Password
    public async Task<ResponseModel<string>> ForgotPasswordAsync(ForgotPasswordRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.ContactNumber))
        {
            return new ResponseModel<string>
            {
                Result = null,
                Message = "Email or Contact Number must be provided.",
                HttpStatusCode = 400
            };
        }

        // Scenario 1: Email is provided
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var user = await _context.applicationUser.SingleOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Result = null,
                    Message = "User with the provided email does not exist.",
                    HttpStatusCode = 404
                };
            }

            // Send an email with a link to create a new password
            string subject = "Reset Your Password";
            string body = $"Hello {user.FirstName},<br><br>" +
                          "Please click the link below to reset your password:<br>" +
                          $"<a href='https://fms.persistentsolutions.co/auth/create-password\r\n?globalId={user.GlobalId}'>Reset Password</a><br><br>" +
                          "Thank you!";

        

            var emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
            if (!emailSent)
            {
                return new ResponseModel<string>
                {
                    Result = null,
                    Message = "Failed to send email. Please try again later.",
                    HttpStatusCode = 500
                };
            }

            return new ResponseModel<string>
            {
                Result = "Email sent successfully. Please check your inbox to reset your password.",
                Message = "Success",
                HttpStatusCode = 200
            };
        }

        // Scenario 2: Contact number is provided
        if (!string.IsNullOrWhiteSpace(request.ContactNumber))
        {
            var user = await _context.applicationUser.SingleOrDefaultAsync(u => u.ContactNumber == request.ContactNumber);
            if (user == null)
            {
                return new ResponseModel<string>
                {
                    Result = null,
                    Message = "User with the provided contact number does not exist.",
                    HttpStatusCode = 404
                };
            }

            // Check if NewPassword and ConfirmPassword are provided
            if (string.IsNullOrWhiteSpace(request.NewPassword) || string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return new ResponseModel<string>
                {
                    Result = null,
                    Message = "New password and confirm password are required.",
                    HttpStatusCode = 400 
                };
            }

            // Validate that the new password matches the confirm password
            if (request.NewPassword != request.ConfirmPassword)
            {
                return new ResponseModel<string>
                {
                    Result = null,
                    Message = "New password and confirm password do not match.",
                    HttpStatusCode = 400
                };
            }

            user.Password = request.NewPassword; 
            _context.applicationUser.Update(user);
            await _context.SaveChangesAsync();

            return new ResponseModel<string>
            {
                Result = "Password updated successfully.",
                Message = "Success",
                HttpStatusCode = 200 
            };
        }

        return new ResponseModel<string>
        {
            Result = null,
            Message = "Invalid request. Please provide either email or contact number.",
            HttpStatusCode = 400
        };
    }
    
    //ValidatePassword
    private string ValidatePassword(string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            return "Password and confirm password are required.";
        }

        if (password != confirmPassword)
        {
            return "Password and confirm password do not match.";
        }

        return string.Empty;  
    }

    //Update User
    public async Task<ResponseModel<ApplicationUserResponse>> UpdateUserAsync(Guid globalId, ApplicationUserRequestDto request)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByGlobalIdAsync(globalId);
        if (user == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found",
                HttpStatusCode = 404
            };
        }

        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
        {
            var existingUserByEmail = await _unitOfWork.ApplicationUserRepository.FindAsync(u => u.Email == request.Email && u.IsDeleted == false && u.Id != user.Id);
            if (existingUserByEmail.Any())
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = "Email already exists. Please use a different email.",
                    HttpStatusCode = 400
                };
            }
        }

        if (!string.IsNullOrWhiteSpace(request.ContactNumber) && request.ContactNumber != user.ContactNumber)
        {
            var existingUserByContact = await _unitOfWork.ApplicationUserRepository.FindAsync(u => u.ContactNumber == request.ContactNumber && u.IsDeleted == false && u.Id != user.Id);
            if (existingUserByContact.Any())
            {
                return new ResponseModel<ApplicationUserResponse>
                {
                    Result = null,
                    Message = "Contact number already exists. Please use a different contact number.",
                    HttpStatusCode = 400
                };
            }
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            request.Password = user.Password;
        }

        _mapper.Map(request, user);

        await _unitOfWork.ApplicationUserRepository.UpdateAsync(user);

        var response = _mapper.Map<ApplicationUserResponse>(user);

        return new ResponseModel<ApplicationUserResponse>
        {
            Result = response,
            Message = "User updated successfully",
            HttpStatusCode = 200
        };
    }

    //SoftDelete User
    public async Task<ResponseModel<ApplicationUserResponse>> DeleteUserAsync(Guid globalId)
    {
        var user = await _unitOfWork.ApplicationUserRepository.GetByGlobalIdAsync(globalId);
        if (user == null)
        {
            return new ResponseModel<ApplicationUserResponse>
            {
                Result = null,
                Message = "User not found",
                HttpStatusCode = 404
            };
        }

        await _unitOfWork.ApplicationUserRepository.SoftDeleteAsync(globalId);

        return new ResponseModel<ApplicationUserResponse>
        {
            Result = null,
            Message = "User deleted successfully",
            HttpStatusCode = 200
        };
    }

    //Search
    public async Task<ResponseModel<PageResult<ApplicationUserResponse>>> Search(SearchRequest searchRequest)
    {
        int page = searchRequest.Page > 0 ? searchRequest.Page : 1;
        int pageSize = searchRequest.PageSize > 0 ? searchRequest.PageSize : 10;

        string searchTerm = string.IsNullOrWhiteSpace(searchRequest.SearchTerm) ? string.Empty : searchRequest.SearchTerm.ToLower();

        var response = await _unitOfWork.ApplicationUserRepository.SimpleSearchEntities(searchTerm, page, pageSize);

        var userIds = response.Result.Result.Select(user => new { user.CreatedBy, user.ModifiedBy })
                                            .SelectMany(ids => new[] { ids.CreatedBy, ids.ModifiedBy })
                                            .Distinct()
                                            .ToList();

        var users = await _context.applicationUser
                                   .Where(u => userIds.Contains(u.Id))
                                   .ToListAsync();

        // Fetch distinct RoleIds from response and get their RoleNames from the Role table
        var roleIds = response.Result.Result
                               .Select(user => user.RoleId)
                               .Distinct()
                               .ToList();

        var roles = await _context.role
                                  .Where(r => roleIds.Contains(r.Id))
                                  .ToListAsync();

        var mappedResponse = new ResponseModel<PageResult<ApplicationUserResponse>>
        {
            Result = new PageResult<ApplicationUserResponse>
            {
                RowCount = response.Result.RowCount,
                PageSize = response.Result.PageSize,
                CurrentPage = response.Result.CurrentPage,
                PageCount = response.Result.PageCount,
                Result = response.Result.Result.Select(user =>
                {
                    var userResponse = _mapper.Map<ApplicationUserResponse>(user);

                    // Map CreatedByName
                    var creator = users.FirstOrDefault(u => u.Id == user.CreatedBy);
                    userResponse.CreatedByName = creator != null ? $"{creator.FirstName} {creator.LastName}" : "Unknown";

                    // Map ModifiedByName
                    var modifier = users.FirstOrDefault(u => u.Id == user.ModifiedBy);
                    userResponse.ModifiedByName = modifier != null ? $"{modifier.FirstName} {modifier.LastName}" : "Unknown";

                    // Map RoleName
                    var role = roles.FirstOrDefault(r => r.Id == user.RoleId);
                    userResponse.RoleName = role != null ? role.Name : "Unknown";

                    return userResponse;
                }).ToList()
            },
            Message = response.Message,
            HttpStatusCode = response.HttpStatusCode
        };

        return mappedResponse;
    }

    //Reset Password    
    public async Task<ResponseModel<string>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.OldPassword))
            return new ResponseModel<string> { Result = null, Message = "Email and old password are required", HttpStatusCode = 400 };

        var user = await _context.applicationUser.SingleOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || user.Password != request.OldPassword)
            return new ResponseModel<string> { Result = null, Message = "Invalid email or password", HttpStatusCode = 400 };

        user.Password = request.NewPassword;
        await _context.SaveChangesAsync();

        return new ResponseModel<string> { Result = null, Message = "Password reset successfully", HttpStatusCode = 200 };
    }




}
