using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http;
using PayRoleSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
 
namespace PayRoleSystem.Controllers
{ 
    public class PagesController : BaseApiController
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            _pageService = pageService;
        }


        //[HttpGet("getAll")]
        //public async Task<ResponseModel<IEnumerable<PageReponseDto>>> GetAllPages()
        //{
        //    var result = await _pageService.GetAllPagesAsync();
        //    return result;
        //}


        //[HttpGet("getById")]
        //public async Task<ResponseModel<PageReponseDto>> GetPageById([FromQuery] int id)
        //{
        //    var result = await _pageService.GetPageByIdAsync(id);
        //    return result;
        //}


        //[HttpPost("create")]
        //public async Task<ResponseModel<PageReponseDto>> CreatePage([FromBody] PageRequestDto request)
        //{
        //    var result = await _pageService.AddPageAsync(request);
        //    return result;
        //}


        //[HttpPut("update")]
        //public async Task<ResponseModel<PageReponseDto>> UpdatePage([FromQuery] int id, [FromBody] PageRequestDto request)
        //{
        //    var result = await _pageService.UpdatePageAsync(id, request);
        //    return result;
        //}


        //[HttpDelete("deleteByGlobalId")]
        //public async Task<ResponseModel<bool>> DeletePage([FromQuery] Guid globalId)
        //{
        //    var result = await _pageService.DeletePageAsync(globalId);
        //    return result;
        //}

        //[HttpGet("getPagesWithControls")]
        //public async Task<ResponseModel<IEnumerable<PageReponseDto>>> GetPagesWithControls()
        //{
        //    var result = await _pageService.GetPagesWithControlsAsync();
        //    return result;
        //}

        [HttpGet("getAll")]
        public async Task<ResponseModel<IEnumerable<PagesResponse>>> GetAllPages()
        {
            var result = await _pageService.GetAllPagesAsync();
            return result;
        }


        [HttpGet("getById")]
        public async Task<ResponseModel<PagesResponse>> GetPageById([FromQuery] int id)
        {
            var result = await _pageService.GetPageByIdAsync(id);
            return result;
        }


        [HttpPost("create")]
        public async Task<ResponseModel<PagesResponse>> CreatePage([FromBody] PagesRequest request)
        {
            var result = await _pageService.AddPageAsync(request);
            return result;
        }


        [HttpPut("update")]
        public async Task<ResponseModel<PagesResponse>> UpdatePage([FromQuery] int id, [FromBody] PagesRequest request)
        {
            var result = await _pageService.UpdatePageAsync(id, request);
            return result;
        }


        [HttpDelete("deleteByGlobalId")]
        public async Task<ResponseModel<bool>> DeletePage([FromQuery] Guid globalId)
        {
            var result = await _pageService.DeletePageAsync(globalId);
            return result;
        }

        [HttpGet("getPagesWithControls")]
        public async Task<ResponseModel<IEnumerable<PagesResponse>>> GetPagesWithControls()
        {
            var result = await _pageService.GetPagesWithControlsAsync();
            return result;
        }

    }
}
