using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class PropertyDto
{
    public string IdProperty { get; set; } = string.Empty;

    [Required]
    public string IdOwner { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Internal code cannot be longer than 50 characters.")]
    public string CodeInternal { get; set; } = string.Empty;

    [Required]
    [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
    public int Year { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Enabled { get; set; }

    // Main image (first image or placeholder)
    public string? MainImage { get; set; }
}
