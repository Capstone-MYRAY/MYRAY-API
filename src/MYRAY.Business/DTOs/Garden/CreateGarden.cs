namespace MYRAY.Business.DTOs.Garden;

public class CreateGarden
{
    public int AreaId { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public decimal? Latitudes { get; set; }
    public decimal? Longitudes { get; set; }
    public string? Address { get; set; }
    public double? LandArea { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
}