using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class PropertyImageDto
{
    public string IdPropertyImage { get; set; } = string.Empty;

    [Required]
    public string IdProperty { get; set; } = string.Empty;

    [Required]
    [Url(ErrorMessage = "File must be a valid URL.")]
    public string File { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

public class CreatePropertyImageDto
{
    [Required(ErrorMessage = "Property ID is required.")]
    public string IdProperty { get; set; } = string.Empty;

    [Required(ErrorMessage = "File URL is required.")]
    [Url(ErrorMessage = "File must be a valid URL.")]
    public string File { get; set; } = string.Empty;

    public bool Enabled { get; set; } = true;
}
