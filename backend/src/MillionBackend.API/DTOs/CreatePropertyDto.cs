using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class CreatePropertyDto
{
    [Required(ErrorMessage = "Owner ID is required.")]
    public string IdOwner { get; set; } = string.Empty;

    [Required(ErrorMessage = "Property name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [StringLength(50, ErrorMessage = "Internal code cannot be longer than 50 characters.")]
    public string? CodeInternal { get; set; }

    [Required(ErrorMessage = "Year is required.")]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int Year { get; set; }
}
