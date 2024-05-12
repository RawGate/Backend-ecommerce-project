using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend_teamwork1.Controllers
{
    public static class ApiResponse
    {
        public static IActionResult Success<T>(T data, string message = "Success")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 200));
        }

        public static IActionResult Created<T>(T data, string message = "Resource Created")
        {
            return new ObjectResult(new ApiResponseTemplate<T>(true, data, message, 201));
        }

        public static IActionResult NotFound(string message = "Resource not found")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 404));
        }

        public static IActionResult Conflict(string message = "Conflict Detected")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 409));
        }

        public static IActionResult BadRequest(string message = "Bad request")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 400));
        }

        public static IActionResult UnAuthorized(string message = "Unauthorized access")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 401));
        }

        public static IActionResult Forbidden(string message = "Forbidden access")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 403));
        }

        public static IActionResult ServerError(string message = "Internal server error")
        {
            return new ObjectResult(new ApiResponseTemplate<object>(false, null, message, 500));
        }
    }

    public class ApiResponseTemplate<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ApiResponseTemplate(bool success, T? data, string message, int statusCode = 200)
        {
            Success = success;
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }
    }
}