using PayRoleSystem.Models;

namespace PayRoleSystem.DTOs.Response
{
    public class ApplicationUserResponse : BasicEntity
    {
        //public int Id { get; set; }
        //public Guid GlobalId { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        public string? ContactNumber { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? PhoneNumber { get; set; }

        public bool? IsActive { get; set; }
        public string? Image { get; set; }

        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }



    }
}
