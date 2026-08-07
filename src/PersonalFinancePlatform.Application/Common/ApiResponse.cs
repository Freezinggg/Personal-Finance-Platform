using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancePlatform.Application.Common
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public static ApiResponse Fail(string message, List<string>? errors = null) => new ApiResponse { Success = false, Message = message, Errors = errors };
    }

    //Generic type to return success with the T data
    public sealed class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }
        public static ApiResponse<T> Ok(T data, string? message = null) => new ApiResponse<T> { Success = true, Data = data, Message = message };
    }

    
}
