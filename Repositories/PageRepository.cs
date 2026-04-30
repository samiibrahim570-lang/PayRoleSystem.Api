using PayRoleSystem.Data;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace PayRoleSystem.Repositories
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        public PageRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<IEnumerable<Page>> GetPagesWithControlsAsync()
        {
            var Page = await _context.page
                 //.Include(p => p.PageControls)
                 //.Include(x => x.RolePagePermissions)
                 .AsNoTracking()
                .ToListAsync();
            return Page;
        }



    }
} 
