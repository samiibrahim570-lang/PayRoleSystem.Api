namespace PayRoleSystem.DTOs.Request
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
