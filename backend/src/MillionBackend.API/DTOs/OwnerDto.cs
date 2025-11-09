using System.ComponentModel.DataAnnotations;

namespace MillionBackend.API.DTOs;

public class OwnerDto
{
    public string IdOwner { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
    public string Address { get; set; } = string.Empty;

    public string Photo { get; set; } = string.Empty;

    [Required]
    public DateTime Birthday { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateOwnerDto
{
    [Required(ErrorMessage = "Owner name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters.")]
    public string Address { get; set; } = string.Empty;

    public string Photo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birthday is required.")]
    public DateTime Birthday { get; set; }
}
