using Minio;
using Minio.DataModel.Args;
using PayRoleSystem.Api.Services.Interfaces;
using PayRoleSystem.Http;

namespace PayRoleSystem.Api.Services
{
    public class MinioService : IMinioService
    {


        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;
        private readonly IConfiguration _configuration;

        public MinioService(IMinioClient minioClient, IConfiguration configuration)
        {
            _minioClient = minioClient ?? throw new ArgumentNullException(nameof(minioClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _bucketName = _configuration["Minio:BucketName"];

            if (string.IsNullOrEmpty(_bucketName))
            {
                throw new InvalidOperationException("Bucket name is not set in the configuration.");
            }
        }

        public async Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder)
        {
            try
            {
                // Check for null or empty file stream
                if (fileStream == null || fileStream.Length == 0)
                {
                    throw new ArgumentException("File stream is empty or null.");
                }

                // Check if the bucket name is properly configured
                if (string.IsNullOrEmpty(_bucketName))
                {
                    throw new InvalidOperationException("Bucket name is not set.");
                }

                // Check if the endpoint is properly configured in the configuration
                var minioEndpoint = _configuration["Minio:Endpoint"];
                if (string.IsNullOrEmpty(minioEndpoint))
                {
                    throw new InvalidOperationException("Minio endpoint is not configured.");
                }

                // Generate a unique name using the current timestamp
                var uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}";

                // Extract the file extension from the original file name
                var fileExtension = Path.GetExtension(fileName);

                // Append the extension to the unique file name
                uniqueFileName = $"{uniqueFileName}{fileExtension}";

                // Create a folder path by combining the folder and file name
                var filePath = $"{folder}/{uniqueFileName}";
                //var filePath = $"{folder}/{fileName}";


                // Perform the file upload
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(filePath)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);

                await _minioClient.PutObjectAsync(putObjectArgs);

                // Generate the file URL (adjusted for folder structure)
                var fileUrl = $"https://s3.persistentsolutions.co/{_bucketName}/{filePath}";

                // Return the response model with the file URL
                return new ResponseModel<string>
                {
                    MessageType = 1,
                    Message = "File uploaded successfully.",
                    Result = filePath,
                    HttpStatusCode = 200,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                // Handle error and return detailed error response
                return new ResponseModel<string>
                {
                    MessageType = 2,
                    Message = "Error occurred while uploading the file.",
                    Result = null,
                    HttpStatusCode = 500,
                    Errors = new List<string> { ex.Message }
                };
            }
        }



    }
}
