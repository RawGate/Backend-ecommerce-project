using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.Helpers
{
    public class SuccessResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Massage { get; set; }
        public T? Data { get; set; }
    }
}