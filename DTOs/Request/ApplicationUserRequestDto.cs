using System.ComponentModel.DataAnnotations;

namespace PayRoleSystem.DTOs.Request
{
    public class ApplicationUserRequestDto
    {
        
        public string? Email { get; set; }        
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        public string? ContactNumber { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }
        public string? Image { get; set; }

        public int? RoleId { get; set; }

    }
}
