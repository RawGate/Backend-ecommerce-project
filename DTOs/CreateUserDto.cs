using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork1.DTOs
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }= string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }= string.Empty;

        public string Address { get; set; }= string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }= string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }= string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        public int Phone { get; set; }
    }
}