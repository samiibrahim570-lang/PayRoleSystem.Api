
using PayRoleSystem.Enum;
using System.Net;

namespace PayRoleSystem.Http
{
    public static class ResponseManager
    {
        public static ResponseModel<T> GenerateResponseModel<T>(HttpStatusCode httpStatusCode, MessageType messageType, T result, IList<string> errors = null)
        {
            ResponseModel<T> responseModel = new ResponseModel<T>
            {
                MessageType = (int)messageType,
                Result = result,
                HttpStatusCode = (int)httpStatusCode,
                Errors = errors ?? new List<string>()
            };

            //if (errors != null && errors.Any())
            //{
            //    responseModel.Errors = errors;
            //}

            return responseModel;
        }
    }
}
