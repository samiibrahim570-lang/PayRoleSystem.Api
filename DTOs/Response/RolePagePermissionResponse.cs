using PayRoleSystem.DTOs.Request;
using PayRoleSystem.Models;

namespace PayRoleSystem.DTOs.Response
{

    public class RolePagePermissionResponseTest : BasicEntity
    {
        public int? RoleId { get; set; }
        public int PageId { get; set; }
        public string? PageName { get; set; }
        public bool? IsCollapsed { get; set; }
        public int? PageParentId { get; set; }
        public string? PagePath { get; set; }
        public string? PageType { get; set; }
        public string? PageIcon { get; set; }
        public bool? PageIsHidden { get; set; }
        public string? PageTranslate { get; set; }
        public string? PageClasses { get; set; }
        public string? PageGroupClasses { get; set; }
        public bool? PageIsExactMatch { get; set; }
        public bool? PageIsTarget { get; set; }
        public bool? PageIsBreadCrumb { get; set; }
        public string? PageDescription { get; set; }
        public string? PageLink { get; set; }
        public int? ParentId { get; set; }
        public bool? IsView { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }
        //public List<RoleControlPermissionResponse> RoleControlPermissions { get; set; }
    }


    public class RolePagePermissionResponse : BasicEntity
    {
        public int? RoleId { get; set; }
        public int PageId { get; set; }
        public PagesResponse? Page { get; set; }
        public int? ParentId { get; set; }
        public bool? IsView { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsDelete { get; set; }

    }
}
