using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Models;

namespace PayRoleSystem.Repositories.Interfaces
{
    public interface IPageRepository : IGenericRepository<Page>
    {
        Task<IEnumerable<Page>> GetPagesWithControlsAsync();
    }
}
