using PayRoleSystem.Models;

namespace PayRoleSystem.DTOs.Request
{
    public class RolePagePermissionRequest : BasicEntity
    {

        public int? RoleId { get; set; }
        public int? PageId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsView { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
        public List<RoleControlPermissionRequest> RoleControlPermissions { get; set; }

    }
}
