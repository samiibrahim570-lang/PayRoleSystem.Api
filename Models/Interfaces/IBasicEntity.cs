using Microsoft.AspNetCore.DataProtection.Internal;

namespace PayRoleSystem.Models.Interfaces
{
    public interface IBasicEntity : IActivate, IDelete, IAudit
    {
    }
}
