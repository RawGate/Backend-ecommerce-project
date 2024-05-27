public class QueryParameters
{
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 3;
    public List<Guid>? SelectedCategories { get; set; } 
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
