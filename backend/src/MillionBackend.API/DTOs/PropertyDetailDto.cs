namespace MillionBackend.API.DTOs;

public class PropertyDetailDto : PropertyDto
{
    public OwnerDto? Owner { get; set; }
    public List<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
    public List<PropertyTraceDto> Traces { get; set; } = new List<PropertyTraceDto>();
}
