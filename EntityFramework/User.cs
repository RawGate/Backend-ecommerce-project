using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.EntityFramework
{
     [Table("Users")]
    public class User
    {

        [Required]
        public Guid UserId { get; set; }

        //[Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone is required")]
        public int Phone { get; set; }
        public bool IsBlocked { get; set; }

        //Relation 1-M
        public ICollection<Order> Orders { get; set; }
    }
}
