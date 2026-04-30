using PayRoleSystem.DTOs.Response;

namespace PayRoleSystem.Models
{
    public class Page : BasicEntity
    {
        public int? ParentId { get; set; }
        public bool? IsCollapsed { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public string? Type { get; set; }
        public string? Icon { get; set; }
        public bool? IsHidden { get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }   
        public int? SeriolNumber { get; set; }

    }
    //public class Page : BasicEntity
    //{
    //    public int? ParentId { get; set; }
    //    public bool? IsCollapsed { get; set; }
    //    public string? Title { get; set; }
    //    public string? Path { get; set; }
    //    public string? Type { get; set; }
    //    public string? Icon { get; set; }
    //    public bool? IsHidden { get; set; }
    //    public string? Description { get; set; }
    //    public string? Link { get; set; }
    //    //public ICollection<RolePagePermission>? RolePagePermissions { get; set; }
    //    //public ICollection<Control>? PageControls { get; set; }

    //}
}
