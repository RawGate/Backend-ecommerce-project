using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    public string Role { get; set; } = "customer"; // Default role is "customer"

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
}