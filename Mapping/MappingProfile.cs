using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Models;
using AutoMapper;

namespace PayRoleSystem.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapping Application User
            CreateMap<ApplicationUserRequestDto, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserResponse>();

            //Mapping Page
            CreateMap<PageRequestDto, Page>();
            CreateMap<Page, PageReponseDto>();

            //Mapping Role
            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();

            CreateMap<RolePagePermissionRequest, RolePagePermission>();
            CreateMap<RolePagePermission, RolePagePermissionResponse>();
        }
    }
}
