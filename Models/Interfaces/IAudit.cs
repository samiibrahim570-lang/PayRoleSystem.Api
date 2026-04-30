namespace PayRoleSystem.Models.Interfaces
{
    public interface IAudit
    {
        long CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        long? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
