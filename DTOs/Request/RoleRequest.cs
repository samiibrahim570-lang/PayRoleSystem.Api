using PayRoleSystem.Models;

namespace PayRoleSystem.DTOs.Request
{
    public class RoleRequest : BasicEntity
    {
        public Guid? GlobalId { get; set; }

        public string Name { get; set; }
        public List<RolePagePermissionRequest> RolePagePermissions { get; set; }
    }


}
