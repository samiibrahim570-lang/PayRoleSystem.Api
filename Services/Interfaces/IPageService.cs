using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;

namespace PayRoleSystem.Services.Interfaces
{
    public interface IPageService
    {
        //Task<ResponseModel<PageReponseDto>> AddPageAsync(PageRequestDto request);
        //Task<ResponseModel<PageReponseDto?>> GetPageByIdAsync(int id);

        //Task<ResponseModel<PageReponseDto?>> UpdatePageAsync(int id, PageRequestDto request);

        //Task<ResponseModel<bool>> DeletePageAsync(Guid globalId);

        //Task<ResponseModel<IEnumerable<PageReponseDto>>> GetAllPagesAsync();
        //Task<ResponseModel<IEnumerable<PageReponseDto>>> GetPagesWithControlsAsync();
        Task<ResponseModel<PagesResponse>> AddPageAsync(PagesRequest request);
        Task<ResponseModel<PagesResponse?>> GetPageByIdAsync(int id);
        Task<ResponseModel<IEnumerable<PagesResponse>>> GetAllPagesAsync();
        Task<ResponseModel<PagesResponse?>> UpdatePageAsync(int id, PagesRequest request);
        Task<ResponseModel<bool>> DeletePageAsync(Guid globalId);
        Task<ResponseModel<IEnumerable<PagesResponse>>> GetPagesWithControlsAsync();

    }
}
