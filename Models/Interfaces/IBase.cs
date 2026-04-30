namespace PayRoleSystem.Models.Interfaces
{
    public interface IBase<TKey>
    {
        TKey Id { get; set; }
    }
}
