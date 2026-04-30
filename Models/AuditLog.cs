namespace PayRoleSystem.Api.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public string? PropertyName { get; set; }
        public string? OriginalValue { get; set; }
        public string? NewValue { get; set; }
        public string? Action { get; set; }
        public long? ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public string? Changes { get; set; }
    }

}
