namespace MYRAY.Business.DTOs.Garden;

public class UpdateGarden
{
    public int Id { get; set; }
    public int AreaId { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public decimal? Latitudes { get; set; }
    public decimal? Longitudes { get; set; }
    public double? LandArea { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
}