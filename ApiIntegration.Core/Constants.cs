using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiIntegration.Core
{
    public static class StatusCodes
    {
        public const string Success = "200";
        public const string Created = "201";
        public const string BadRequest = "400";
        public const string NotFound = "404";
        public const string Conflict = "409";
        public const string InternalServerError = "500";
    }
    public static class StatusMessages
    {
        public const string Success = "Request successful";
        public const string Created = "Resource created successfully";
        public const string BadRequest = "request failed";
        public const string NotFound = "Resource not found";
        public const string Conflict = "Conflict detected";
        public const string ServerError = "An internal server error occurred";
    }


}
