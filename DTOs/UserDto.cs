using System;
using System.ComponentModel.DataAnnotations;

namespace backend_teamwork1.DTOs
{
    public class UserDto
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(admin|customer)$", ErrorMessage = "Role must be either 'admin' or 'customer'")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }= string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public int Phone { get; set; }
        public bool IsBlocked { get; set; }
    }
}
