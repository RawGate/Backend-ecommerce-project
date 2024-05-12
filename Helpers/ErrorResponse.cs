using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.Helpers
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public string? Massage { get; set; }
    }
}
