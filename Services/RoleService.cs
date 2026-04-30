using PayRoleSystem.Models;
using PayRoleSystem.DTOs.Request;
using PayRoleSystem.Services.Interfaces;
using PayRoleSystem.UOW;
using AutoMapper;
using PayRoleSystem.Http;
using PayRoleSystem.DTOs.Response;
using System.Security.Claims;
using PayRoleSystem.Data;




public class PageDetails
{
    public int? ParentId { get; set; }
    public bool? IsCollapsed { get; set; }
    public string? Title { get; set; }
    public string? Path { get; set; }
    public string? Type { get; set; }
    public string? Icon { get; set; }
    public bool? IsHidden { get; set; }
    public string? Translate { get; set; }
    public string? Classes { get; set; }
    public string? GroupClasses { get; set; }
    public bool? IsExactMatch { get; set; }
    public bool? IsTarget { get; set; }
    public bool? IsBreadCrumb { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
}

namespace PayRoleSystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;



        public RoleService(ApplicationDbContext context,IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        //public async Task<ResponseModel<IList<RoleResponse>>> SaveRoleAsync(RoleRequest roleRequest)
        //{
        //    var response = new ResponseModel<IList<RoleResponse>>();

        //    try
        //    {
        //        // Step 1: Check for duplicate Role.Name
        //        var existingRole = await _unitOfWork.RoleRepository.GetByNameAsync(roleRequest.Name);
        //        if (existingRole != null && (!roleRequest.GlobalId.HasValue || existingRole.GlobalId != roleRequest.GlobalId.Value))
        //        {
        //            response.MessageType = 0; // Error
        //            response.Message = "A role with the same name already exists.";
        //            response.HttpStatusCode = 400;
        //            return response;
        //        }

        //        // Step 2: Retrieve or create Role
        //        var role = roleRequest.GlobalId.HasValue
        //            ? await _unitOfWork.RoleRepository.GetByGlobalIdAsync(roleRequest.GlobalId.Value)
        //            : new Role { CreatedOn = DateTime.UtcNow };

        //        if (role == null)
        //            throw new Exception("Failed to retrieve or create Role.");

        //        role.Name = roleRequest.Name;

        //        if (!roleRequest.GlobalId.HasValue)
        //            await _unitOfWork.RoleRepository.AddAsync(role);

        //        await _unitOfWork.SaveChangesAsync();

        //        // Step 3: Delete existing RoleControlPermissions and RolePagePermissions
        //        await _unitOfWork.RoleControlPermissionRepository.PermanentDeleteAsyncById(role.Id, nameof(RoleControlPermission.RoleId));
        //        await _unitOfWork.RolePagePermissionRepository.PermanentDeleteAsyncById(role.Id, nameof(RolePagePermission.RoleId));

        //        // Step 4: Add new RolePagePermissions and associated RoleControlPermissions
        //        if (roleRequest.RolePagePermissions?.Any() == true)
        //        {
        //            var parentIds = new Dictionary<int, int>();

        //            foreach (var pagePermissionRequest in roleRequest.RolePagePermissions)
        //            {
        //                if (!pagePermissionRequest.PageId.HasValue)
        //                    throw new ArgumentException("PageId cannot be null.");

        //                int pageId = pagePermissionRequest.PageId.Value;

        //                int parentId = 0;
        //                if (pagePermissionRequest.ParentId.HasValue)
        //                {
        //                    if (!parentIds.TryGetValue(pagePermissionRequest.ParentId.Value, out parentId))
        //                    {
        //                        parentId = 0;
        //                    }
        //                }

        //                var rolePagePermission = new RolePagePermission
        //                {
        //                    RoleId = role.Id,
        //                    PageId = pageId,
        //                    ParentId = parentId,
        //                    IsView = pagePermissionRequest.IsView,
        //                    IsAdd = pagePermissionRequest.IsAdd,
        //                    IsEdit = pagePermissionRequest.IsEdit,
        //                    IsDeleted = pagePermissionRequest.IsDeleted
        //                };

        //                await _unitOfWork.RolePagePermissionRepository.AddAsync(rolePagePermission);
        //                await _unitOfWork.SaveChangesAsync();

        //                parentIds[pageId] = rolePagePermission.Id;

        //                if (pagePermissionRequest.RoleControlPermissions?.Any() == true)
        //                {
        //                    rolePagePermission.RoleControlPermissions = pagePermissionRequest.RoleControlPermissions.Select(cp => new RoleControlPermission
        //                    {
        //                        RoleId = role.Id,
        //                        PageId = cp.PageId ?? throw new ArgumentException("PageId cannot be null."),
        //                        ControlId = cp.ControlId ?? throw new ArgumentException("ControlId cannot be null."),
        //                        IsView = cp.IsView
        //                    }).ToList();
        //                }
        //            }
        //        }

        //        await _unitOfWork.SaveChangesAsync();

        //        // Step 5: Retrieve Role with related data
        //        var roleResponse = await GetByRoleIdPermissionsAsync(role.Id);

        //        // Step 6: Success response
        //        response.MessageType = 1; // Success
        //        response.Message = "Role saved or updated successfully.";
        //        response.HttpStatusCode = 200;
        //        response.Result = roleResponse.Result;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MessageType = 0; // Error
        //        response.Message = "An error occurred while saving the role.";
        //        response.HttpStatusCode = 500;
        //        response.Errors = new List<string>
        //{
        //    ex.Message,
        //    ex.InnerException?.Message
        //};
        //    }

        //    return response;
        //}

        //public async Task<ResponseModel<IList<RoleResponse>>> GetByRoleIdPermissionsAsync(int roleId)
        //{
        //    var roles = _unitOfWork.RoleRepository.GetListByIdAsync(roleId).ToList();

        //    var response = _unitOfWork.FunctionRepository.MapList<Role, RoleResponse>(roles);

        //    foreach (var role in response)
        //    {
        //        foreach (var pagePermission in role.RolePagePermissions)
        //        {
        //            var pageDetails = await _getPageDetailsById(pagePermission.PageId);

        //            pagePermission.PageName = pageDetails.Title;
        //            pagePermission.IsCollapsed = pageDetails.IsCollapsed;
        //            pagePermission.PageGroupClasses = pageDetails.GroupClasses;
        //            pagePermission.PageLink = pageDetails.Link;
        //            pagePermission.PagePath = pageDetails.Path;
        //            pagePermission.PageParentId = pageDetails.ParentId;
        //            pagePermission.PageClasses = pageDetails.Classes;
        //            pagePermission.PageIsBreadCrumb = pageDetails.IsBreadCrumb;
        //            pagePermission.PageDescription = pageDetails.Description;
        //            pagePermission.PageIcon = pageDetails.Icon;
        //            pagePermission.PageIsExactMatch = pageDetails.IsExactMatch;
        //            pagePermission.PageIsHidden = pageDetails.IsHidden;
        //            pagePermission.PageIsTarget = pageDetails.IsTarget;
        //            pagePermission.PageType = pageDetails.Type;
        //            pagePermission.PageTranslate = pageDetails.Translate;



        //        }
        //    }

        //    return new ResponseModel<IList<RoleResponse>>
        //    {
        //        Result = response.ToList(),  
        //        Message = "Roles retrieved successfully.",
        //        HttpStatusCode = 200
        //    };
        //}

        ////Get Page Details
        //private async Task<PageDetails> _getPageDetailsById(int pageId)
        //{
        //    var _page = await _unitOfWork.PageRepository.GetByIdAsync(pageId);

        //    return new PageDetails
        //    {
        //        Title = _page?.Title ?? "",
        //        IsCollapsed = _page?.IsCollapsed ,
        //        Translate = _page?.Translate ,
        //        Type = _page?.Type ,
        //        IsTarget = _page?.IsTarget ,
        //        IsHidden = _page?.IsHidden ,
        //        IsExactMatch = _page?.IsExactMatch ,
        //        Classes = _page?.Classes ,
        //        Description = _page?.Description ,
        //        GroupClasses = _page?.GroupClasses ,
        //        Icon = _page?.Icon ,
        //        IsBreadCrumb = _page?.IsBreadCrumb ,
        //        Link = _page?.Link ,
        //        ParentId = _page?.ParentId ,
        //        Path = _page?.Path 

        //    };
        //}


        ////GetRoleBy-Token
        //public async Task<ResponseModel<RoleResponse>> GetRoleByTokenAsync()
        //{
        //    var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return new ResponseModel<RoleResponse>
        //        {
        //            Result = null,
        //            Message = "Token is missing in the request header.",
        //            HttpStatusCode = 400
        //        };
        //    }

        //    var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    if (!int.TryParse(userIdClaim, out var userId))
        //    {
        //        return new ResponseModel<RoleResponse>
        //        {
        //            Result = null,
        //            Message = "Invalid user ID in token.",
        //            HttpStatusCode = 400
        //        };
        //    }

        //    var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
        //    if (user == null)
        //    {
        //        return new ResponseModel<RoleResponse>
        //        {
        //            Result = null,
        //            Message = "User not found.",
        //            HttpStatusCode = 404
        //        };
        //    }

        //    var rolePermissionsResponse = await GetByRoleIdPermissionsAsync(user.RoleId ?? 0);
        //    if (rolePermissionsResponse.HttpStatusCode != 200)
        //    {
        //        return new ResponseModel<RoleResponse>
        //        {
        //            Result = null,
        //            Message = "Failed to retrieve role permissions: " + rolePermissionsResponse.Message,
        //            HttpStatusCode = rolePermissionsResponse.HttpStatusCode
        //        };
        //    }

        //    // Map the first role response if roles exist
        //    var response = rolePermissionsResponse.Result.FirstOrDefault();
        //    if (response == null)
        //    {
        //        return new ResponseModel<RoleResponse>
        //        {
        //            Result = null,
        //            Message = "Role permissions not found.",
        //            HttpStatusCode = 404
        //        };
        //    }

        //    return new ResponseModel<RoleResponse>
        //    {
        //        Result = response,
        //        Message = "User and role permissions retrieved successfully.",
        //        HttpStatusCode = 200
        //    };
        //}


        //public async Task<ResponseModel<IList<RolesResponse>>> GetAllRolesAsync()
        //{
        //    var response = new ResponseModel<IList<RolesResponse>>();

        //    try
        //    {
        //        // Retrieve all roles from the database
        //        var roles = await _unitOfWork.RoleRepository.GetAllAsync();

        //        // Project the roles to a simplified response model
        //        var roleResponses = roles.Select(role => new RolesResponse
        //        {
        //            Id = role.Id,
        //            GlobalId = role.GlobalId,
        //            Name = role.Name
        //        }).ToList();

        //        response.Result = roleResponses;
        //        response.MessageType = 1; // Success
        //        response.Message = "Roles retrieved successfully.";
        //        response.HttpStatusCode = 200;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MessageType = 0; // Error
        //        response.Message = "An error occurred while retrieving roles.";
        //        response.HttpStatusCode = 500;
        //        response.Errors = new List<string>
        //{
        //    ex.Message,
        //    ex.InnerException?.Message
        //};
        //    }

        //    return response;
        //}



        //public async Task<ResponseModel<IList<RoleWithUsersResponse>>> GetAllRolesWithUsersAsync()
        //{
        //    var response = new ResponseModel<IList<RoleWithUsersResponse>>();

        //    try
        //    {
        //        // Retrieve all roles from the database
        //        var roles = await _unitOfWork.RoleRepository.GetAllAsync();

        //        var roleWithUsersResponses = new List<RoleWithUsersResponse>();

        //        foreach (var role in roles)
        //        {
        //            // Get users associated with this role
        //            var users = await _unitOfWork.ApplicationUserRepository.GetListByRoleIdAsync(role.Id);

        //            var roleWithUsersResponse = new RoleWithUsersResponse
        //            {
        //                Id = role.Id,
        //                Name = role.Name,
        //                AssignedUsers = users.Select(user => new ApplicationUserAlongRole
        //                {
        //                    Id = user.Id,
        //                    FirstName = user.FirstName,
        //                    LastName = user.LastName,
        //                    Email = user.Email,
        //                    RoleId = role.Id,
        //                }).ToList()
        //            };

        //            roleWithUsersResponses.Add(roleWithUsersResponse);
        //        }

        //        response.Result = roleWithUsersResponses;
        //        response.MessageType = 1; // Success
        //        response.Message = "Roles with assigned users retrieved successfully.";
        //        response.HttpStatusCode = 200;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.MessageType = 0; // Error
        //        response.Message = "An error occurred while retrieving roles with users.";
        //        response.HttpStatusCode = 500;
        //        response.Errors = new List<string>
        //{
        //    ex.Message,
        //    ex.InnerException?.Message
        //};
        //    }

        //    return response;
        //}

        public async Task<ResponseModel<RoleResponse>> SaveRoleAsync(RoleRequest roleRequest)
        {
            var response = new ResponseModel<RoleResponse>();

            try
            {
                // Step 1: Check for duplicate Role.Name
                var existingRole = await _unitOfWork.RoleRepository.GetByNameAsync(roleRequest.Name);
                if (existingRole != null && (!roleRequest.GlobalId.HasValue || existingRole.GlobalId != roleRequest.GlobalId.Value))
                {
                    response.MessageType = 0; // Error
                    response.Message = "A role with the same name already exists.";
                    response.HttpStatusCode = 400;
                    return response;
                }

                // Step 2: Retrieve or create Role
                var role = roleRequest.GlobalId.HasValue
                    ? await _unitOfWork.RoleRepository.GetByGlobalIdAsync(roleRequest.GlobalId.Value)
                    : new Role { CreatedOn = DateTime.UtcNow };

                if (role == null)
                    throw new Exception("Failed to retrieve or create Role.");

                role.Name = roleRequest.Name;

                if (!roleRequest.GlobalId.HasValue)
                    await _unitOfWork.RoleRepository.AddAsync(role);

                await _unitOfWork.SaveChangesAsync();

                // Step 3: Delete existing RoleControlPermissions and RolePagePermissions
                await _unitOfWork.RoleControlPermissionRepository.PermanentDeleteAsyncById(role.Id, nameof(RoleControlPermission.RoleId));
                await _unitOfWork.RolePagePermissionRepository.PermanentDeleteAsyncById(role.Id, nameof(RolePagePermission.RoleId));

                // Step 4: Add new RolePagePermissions and associated RoleControlPermissions
                if (roleRequest.RolePagePermissions?.Any() == true)
                {
                    var parentIds = new Dictionary<int, int>();

                    foreach (var pagePermissionRequest in roleRequest.RolePagePermissions)
                    {
                        if (!pagePermissionRequest.PageId.HasValue)
                            throw new ArgumentException("PageId cannot be null.");

                        int pageId = pagePermissionRequest.PageId.Value;

                        int parentId = 0;
                        if (pagePermissionRequest.ParentId.HasValue)
                        {
                            if (!parentIds.TryGetValue(pagePermissionRequest.ParentId.Value, out parentId))
                            {
                                parentId = 0;
                            }
                        }

                        var rolePagePermission = new RolePagePermission
                        {
                            RoleId = role.Id,
                            PageId = pageId,
                            ParentId = parentId,
                            IsView = pagePermissionRequest.IsView,
                            IsAdd = pagePermissionRequest.IsAdd,
                            IsEdit = pagePermissionRequest.IsEdit,
                            IsDelete = pagePermissionRequest.IsDelete ?? false
                        };

                        await _unitOfWork.RolePagePermissionRepository.AddAsync(rolePagePermission);
                        await _unitOfWork.SaveChangesAsync();

                        parentIds[pageId] = rolePagePermission.Id;

                        //if (pagePermissionRequest.RoleControlPermissions?.Any() == true)
                        //{
                        //    rolePagePermission.RoleControlPermissions = pagePermissionRequest.RoleControlPermissions.Select(cp => new RoleControlPermission
                        //    {
                        //        RoleId = role.Id,
                        //        PageId = cp.PageId ?? throw new ArgumentException("PageId cannot be null."),
                        //        ControlId = cp.ControlId ?? throw new ArgumentException("ControlId cannot be null."),
                        //        IsView = cp.IsView
                        //    }).ToList();
                        //}
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                // Step 5: Retrieve Role with related data
                var roleResponse = await GetByRoleIdPermissionsAsync(role.Id);

                // Step 6: Success response
                response.MessageType = 1; // Success
                response.Message = "Role saved or updated successfully.";
                response.HttpStatusCode = 200;
                response.Result = roleResponse.Result;
            }
            catch (Exception ex)
            {
                response.MessageType = 0; // Error
                response.Message = "An error occurred while saving the role.";
                response.HttpStatusCode = 500;
                response.Errors = new List<string>
        {
            ex.Message,
            ex.InnerException?.Message
        };
            }

            return response;
        }


        //Sami Add method For getById Role
        public async Task<ResponseModel<RoleResponse>> GetByRoleIdPermissionsAsync(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetListByIdAsync(roleId);

            if (role == null)
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = $"Role with ID {roleId} not found.",
                    HttpStatusCode = 404
                };
            }

            var response = _mapper.Map<RoleResponse>(role);
            //var response = _unitOfWork.FunctionRepository.MapSingle<Role, RoleResponse>(role);

            //foreach (var pagePermission in response.RolePagePermissions)
            //{
            //    var pageDetails = await _getPageDetailsById(pagePermission.PageId);

            //    if (pageDetails != null)
            //    {
            //        pagePermission.PageName = pageDetails.Title;
            //        pagePermission.IsCollapsed = pageDetails.IsCollapsed;
            //        pagePermission.PageGroupClasses = pageDetails.GroupClasses;
            //        pagePermission.PageLink = pageDetails.Link;
            //        pagePermission.PagePath = pageDetails.Path;
            //        pagePermission.PageParentId = pageDetails.ParentId;
            //        pagePermission.PageClasses = pageDetails.Classes;
            //        pagePermission.PageIsBreadCrumb = pageDetails.IsBreadCrumb;
            //        pagePermission.PageDescription = pageDetails.Description;
            //        pagePermission.PageIcon = pageDetails.Icon;
            //        pagePermission.PageIsExactMatch = pageDetails.IsExactMatch;
            //        pagePermission.PageIsHidden = pageDetails.IsHidden;
            //        pagePermission.PageIsTarget = pageDetails.IsTarget;
            //        pagePermission.PageType = pageDetails.Type;
            //        pagePermission.PageTranslate = pageDetails.Translate;
            //    }
            //}

            return new ResponseModel<RoleResponse>
            {
                Result = response,
                Message = "Role permissions retrieved successfully.",
                HttpStatusCode = 200
            };
        }



        ////Get Page Details
        //private async Task<PageDetails> _getPageDetailsById(int pageId)
        //{
        //    var _page = await _unitOfWork.RoleRepository.GetByIdAsync(pageId);

        //    return new PageDetails
        //    {
        //        Title = _page?.Title ?? "",
        //        IsCollapsed = _page?.IsCollapsed,
        //        Type = _page?.Type,
        //        IsHidden = _page?.IsHidden,
        //        Description = _page?.Description,
        //        Icon = _page?.Icon,
        //        Link = _page?.Link,
        //        ParentId = _page?.ParentId,
        //        Path = _page?.Path

        //    };
        //}
        //GetRoleBy-Token
        public async Task<ResponseModel<RoleResponse>> GetRoleByTokenAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = "Token is missing in the request header.",
                    HttpStatusCode = 400
                };
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = "Invalid user ID in token.",
                    HttpStatusCode = 400
                };
            }

            var user = await _unitOfWork.ApplicationUserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = "User not found.",
                    HttpStatusCode = 404
                };
            }

            var rolePermissionsResponse = await GetByRoleIdPermissionsAsync(user.RoleId ?? 0);
            if (rolePermissionsResponse.HttpStatusCode != 200)
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = "Failed to retrieve role permissions: " + rolePermissionsResponse.Message,
                    HttpStatusCode = rolePermissionsResponse.HttpStatusCode
                };
            }

            // Map the first role response if roles exist
            var response = rolePermissionsResponse.Result;
            if (response == null)
            {
                return new ResponseModel<RoleResponse>
                {
                    Result = null,
                    Message = "Role permissions not found.",
                    HttpStatusCode = 404
                };
            }

            return new ResponseModel<RoleResponse>
            {
                Result = response,
                Message = "User and role permissions retrieved successfully.",
                HttpStatusCode = 200
            };
        }
        public async Task<ResponseModel<IList<RoleResponse>>> GetAllRolesAsync()
        {
            var response = new ResponseModel<IList<RoleResponse>>();

            try
            {
                // Retrieve all roles from the database
                var roles = await _unitOfWork.RoleRepository.GetAllAsync();

                // Project the roles to a simplified response model
                var roleResponses = roles.Select(role => new RoleResponse
                {
                    Id = role.Id,
                    GlobalId = role.GlobalId,
                    Name = role.Name
                }).ToList();

                response.Result = roleResponses;
                response.MessageType = 1; 
                response.Message = "Roles retrieved successfully.";
                response.HttpStatusCode = 200;
            }
            catch (Exception ex)
            {
                response.MessageType = 0; 
                response.Message = "An error occurred while retrieving roles.";
                response.HttpStatusCode = 500;
                response.Errors = new List<string>
        {
            ex.Message,
            ex.InnerException?.Message
        };
            }

            return response;
        }

        public async Task<ResponseModel<IList<RoleWithUsersResponse>>> GetAllRolesWithUsersAsync()
        {
            var response = new ResponseModel<IList<RoleWithUsersResponse>>();

            try
            {
                // Retrieve all roles from the database
                var roles = await _unitOfWork.RoleRepository.GetAllAsync();

                var roleWithUsersResponses = new List<RoleWithUsersResponse>();

                foreach (var role in roles)
                {
                    // Get users associated with this role
                    var users = await _unitOfWork.ApplicationUserRepository.GetListByRoleIdAsync(role.Id);

                    var roleWithUsersResponse = new RoleWithUsersResponse
                    {
                        Id = role.Id,
                        Name = role.Name,
                        AssignedUsers = users.Select(user => new ApplicationUserAlongRole
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            RoleId = role.Id,
                        }).ToList()
                    };

                    roleWithUsersResponses.Add(roleWithUsersResponse);
                }

                response.Result = roleWithUsersResponses;
                response.MessageType = 1; // Success
                response.Message = "Roles with assigned users retrieved successfully.";
                response.HttpStatusCode = 200;
            }
            catch (Exception ex)
            {
                response.MessageType = 0; // Error
                response.Message = "An error occurred while retrieving roles with users.";
                response.HttpStatusCode = 500;
                response.Errors = new List<string>
        {
            ex.Message,
            ex.InnerException?.Message
        };
            }

            return response;
        }

    }
}
