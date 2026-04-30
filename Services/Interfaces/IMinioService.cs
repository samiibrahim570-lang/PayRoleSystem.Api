using PayRoleSystem.Http;

namespace PayRoleSystem.Api.Services.Interfaces
{
    public interface IMinioService
    {

        Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder);

    }
}
