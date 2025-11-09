using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class PropertyTraceDto
{
    public string IdPropertyTrace { get; set; } = string.Empty;

    [Required]
    public DateTime DateSale { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
    public decimal Value { get; set; }

    [Required]
    [Range(0.00, double.MaxValue, ErrorMessage = "Tax must be 0 or greater.")]
    public decimal Tax { get; set; }

    [Required]
    public string IdProperty { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

public class CreatePropertyTraceDto
{
    [Required(ErrorMessage = "Sale date is required.")]
    public DateTime DateSale { get; set; }

    [Required(ErrorMessage = "Trace name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Value is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0.")]
    public decimal Value { get; set; }

    [Required(ErrorMessage = "Tax is required.")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Tax must be 0 or greater.")]
    public decimal Tax { get; set; }

    [Required(ErrorMessage = "Property ID is required.")]
    public string IdProperty { get; set; } = string.Empty;
}
