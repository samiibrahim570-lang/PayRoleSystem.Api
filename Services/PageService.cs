using PayRoleSystem.Data;
using PayRoleSystem.DTOs.Request;
using PayRoleSystem.DTOs.Response;
using PayRoleSystem.Http; // Common response model
using PayRoleSystem.Models;
using PayRoleSystem.Services.Interfaces;
using PayRoleSystem.UOW;
using AutoMapper;

namespace PayRoleSystem.Services
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public PageService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        //// INSERT (Create)
        //public async Task<ResponseModel<PageReponseDto>> AddPageAsync(PageRequestDto request)
        //{
        //    var entity = _mapper.Map<Page>(request);
        //    await _unitOfWork.PageRepository.AddAsync(entity);

        //    var response = _mapper.Map<PageReponseDto>(entity);
        //    return new ResponseModel<PageReponseDto>
        //    {
        //        Result = response,
        //        Message = "Page added successfully",
        //        HttpStatusCode = 201
        //    };
        //}

        //// GET (Read)
        //public async Task<ResponseModel<PageReponseDto?>> GetPageByIdAsync(int id)
        //{
        //    var entity = await _unitOfWork.PageRepository.GetByIdAsync(id);
        //    if (entity == null)
        //    {
        //        return new ResponseModel<PageReponseDto?>
        //        {
        //            Result = null,
        //            Message = "Page not found",
        //            HttpStatusCode = 404
        //        };
        //    }

        //    var response = _mapper.Map<PageReponseDto>(entity);
        //    return new ResponseModel<PageReponseDto?>
        //    {
        //        Result = response,
        //        Message = "Page retrieved successfully",
        //        HttpStatusCode = 200
        //    };
        //}


        //// GET ALL (Retrieve List)
        //public async Task<ResponseModel<IEnumerable<PageReponseDto>>> GetAllPagesAsync()
        //{
        //    var entities = await _unitOfWork.PageRepository.GetAllAsync();
        //    var response = _mapper.Map<IEnumerable<PageReponseDto>>(entities);

        //    return new ResponseModel<IEnumerable<PageReponseDto>>
        //    {
        //        Result = response,
        //        Message = "Pages retrieved successfully",
        //        HttpStatusCode = 200
        //    };
        //}


        //// UPDATE
        //public async Task<ResponseModel<PageReponseDto?>> UpdatePageAsync(int id, PageRequestDto request)
        //{
        //    var entity = await _unitOfWork.PageRepository.GetByIdAsync(id);
        //    if (entity == null)
        //    {
        //        return new ResponseModel<PageReponseDto?>
        //        {
        //            Result = null,
        //            Message = "Page not found",
        //            HttpStatusCode = 404
        //        };
        //    }

        //    _mapper.Map(request, entity);
        //    _unitOfWork.PageRepository.UpdateAsync(entity);


        //    var response = _mapper.Map<PageReponseDto>(entity);
        //    return new ResponseModel<PageReponseDto?>
        //    {
        //        Result = response,
        //        Message = "Page updated successfully",
        //        HttpStatusCode = 200
        //    };
        //}

        //// DELETE
        //public async Task<ResponseModel<bool>> DeletePageAsync(Guid globalId)
        //{
        //    var entity = await _unitOfWork.PageRepository.GetByGlobalIdAsync(globalId);
        //    if (entity == null)
        //    {
        //        return new ResponseModel<bool>
        //        {
        //            Result = false,
        //            Message = "Page not found",
        //            HttpStatusCode = 404
        //        };
        //    }

        //    _unitOfWork.PageRepository.SoftDeleteAsync(globalId);


        //    return new ResponseModel<bool>
        //    {
        //        Result = true,
        //        Message = "Page deleted successfully",
        //        HttpStatusCode = 200
        //    };
        //}

        //public async Task<ResponseModel<IEnumerable<PageReponseDto>>> GetPagesWithControlsAsync()
        //{
        //    // Fetch pages and their controls
        //    var entities = await _unitOfWork.PageRepository.GetPagesWithControlsAsync();

        //    // Inspect the entities to ensure PageControls are not empty
        //    foreach (var entity in entities)
        //    {
        //        Console.WriteLine($"Page {entity.Title} has {entity.PageControls?.Count() ?? 0} PageControls.");
        //    }

        //    // Map to response DTO
        //    var response = _mapper.Map<IEnumerable<PageReponseDto>>(entities);

        //    return new ResponseModel<IEnumerable<PageReponseDto>>
        //    {
        //        Result = response,
        //        Message = "Pages with PageControls retrieved successfully",
        //        HttpStatusCode = 200
        //    };
        //}



        // INSERT (Create)
        public async Task<ResponseModel<PagesResponse>> AddPageAsync(PagesRequest request)
        {
            var entity = _mapper.Map<Page>(request);
            await _unitOfWork.PageRepository.AddAsync(entity);

            var response = _mapper.Map<PagesResponse>(entity);
            return new ResponseModel<PagesResponse>
            {
                Result = response,
                Message = "Page added successfully",
                HttpStatusCode = 201
            };
        }

        // GET (Read)
        public async Task<ResponseModel<PagesResponse?>> GetPageByIdAsync(int id)
        {
            var entity = await _unitOfWork.PageRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ResponseModel<PagesResponse?>
                {
                    Result = null,
                    Message = "Page not found",
                    HttpStatusCode = 404
                };
            }

            var response = _mapper.Map<PagesResponse>(entity);
            return new ResponseModel<PagesResponse?>
            {
                Result = response,
                Message = "Page retrieved successfully",
                HttpStatusCode = 200
            };
        }


        // GET ALL (Retrieve List)
        public async Task<ResponseModel<IEnumerable<PagesResponse>>> GetAllPagesAsync()
        {
            var entities = await _unitOfWork.PageRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<PagesResponse>>(entities);

            return new ResponseModel<IEnumerable<PagesResponse>>
            {
                Result = response,
                Message = "Pages retrieved successfully",
                HttpStatusCode = 200
            };
        }


        // UPDATE
        public async Task<ResponseModel<PagesResponse?>> UpdatePageAsync(int id, PagesRequest request)
        {
            var entity = await _unitOfWork.PageRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ResponseModel<PagesResponse?>
                {
                    Result = null,
                    Message = "Page not found",
                    HttpStatusCode = 404
                };
            }

            _mapper.Map(request, entity);
            _unitOfWork.PageRepository.UpdateAsync(entity);


            var response = _mapper.Map<PagesResponse>(entity);
            return new ResponseModel<PagesResponse?>
            {
                Result = response,
                Message = "Page updated successfully",
                HttpStatusCode = 200
            };
        }

        // DELETE
        public async Task<ResponseModel<bool>> DeletePageAsync(Guid globalId)
        {
            var entity = await _unitOfWork.PageRepository.GetByGlobalIdAsync(globalId);
            if (entity == null)
            {
                return new ResponseModel<bool>
                {
                    Result = false,
                    Message = "Page not found",
                    HttpStatusCode = 404
                };
            }

            _unitOfWork.PageRepository.SoftDeleteAsync(globalId);


            return new ResponseModel<bool>
            {
                Result = true,
                Message = "Page deleted successfully",
                HttpStatusCode = 200
            };
        }

        public async Task<ResponseModel<IEnumerable<PagesResponse>>> GetPagesWithControlsAsync()
        {
            // Fetch pages and their controls
            var entities = await _unitOfWork.PageRepository.GetPagesWithControlsAsync();
            foreach (var entity in entities)
            {
            }

            var response = _mapper.Map<IEnumerable<PagesResponse>>(entities);

            return new ResponseModel<IEnumerable<PagesResponse>>
            {
                Result = response,
                Message = "Pages with PageControls retrieved successfully",
                HttpStatusCode = 200
            };
        }

    }

}
