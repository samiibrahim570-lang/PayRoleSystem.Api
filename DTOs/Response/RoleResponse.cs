using PayRoleSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PayRoleSystem.DTOs.Response
{
    //public class RoleResponse : BasicEntity
    //{
    //    public int? Id { get; set; }
    //    public Guid? GlobalId { get; set; }
    //    public string? Name { get; set; }
    //    public List<RoleControlPermissionResponse> RolePagePermissions { get; set; }
    //}
    public class RoleResponse : BasicEntity
    {
        public string? Name { get; set; }
        public IList<RolePagePermissionResponse>? RolePagePermissions { get; set; }
    }


    public class RoleWithUsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ApplicationUserAlongRole> AssignedUsers { get; set; }
    }

    public class ApplicationUserAlongRole
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }

    }

}
