using PayRoleSystem.DTOs.Response;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PayRoleSystem.Models
{
    public class RolePagePermission : BasicEntity
    {
        //public Role? Role { get; set; }
        public int? RoleId { get; set; }
        public int PageId { get; set; }
        public Page? Page { get; set; }
        public int? ParentId { get; set; }
        public bool? IsView { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }


        //public List<RoleControlPermission> RoleControlPermissions { get; set; }

    }
}
