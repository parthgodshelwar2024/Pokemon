using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    //internal class ApiResponse
    //{
    //}
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string CorrelationId { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message, string correlationId)
            => new() { Success = true, Data = data, Message = message, CorrelationId = correlationId };

        public static ApiResponse<T> Failure(string message, string correlationId)
            => new() { Success = false, Message = message, CorrelationId = correlationId };
    }






}



