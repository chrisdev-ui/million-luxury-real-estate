namespace MillionBackend.Core.Models;

public class PropertyQueryParameters
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? Year { get; set; }
    public string? CodeInternal { get; set; }
    public string? IdOwner { get; set; }
    public bool? Enabled { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Related data inclusion flags
    public bool IncludeOwner { get; set; } = false;
    public bool IncludeImages { get; set; } = false;
    public bool IncludeTraces { get; set; } = false;

    // Sorting
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}
