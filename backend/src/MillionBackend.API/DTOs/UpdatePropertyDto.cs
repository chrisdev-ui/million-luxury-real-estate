using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class UpdatePropertyDto
{
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string? Name { get; set; }

    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
    public string? Address { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal? Price { get; set; }

    public bool? Enabled { get; set; }
}
