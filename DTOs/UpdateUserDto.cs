using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork1.DTOs
{
    public class UpdateUserDto
    {
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; }= string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Phone { get; set; }
    }
}