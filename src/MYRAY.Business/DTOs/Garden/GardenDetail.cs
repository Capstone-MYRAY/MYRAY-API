namespace MYRAY.Business.DTOs.Garden;

public class GardenDetail
{
    public int Id { get; set; }
    public int AreaId { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; }
    public decimal? Latitudes { get; set; }
    public decimal? Longitudes { get; set; }
    public string? Address { get; set; }
    public double? LandArea { get; set; }
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreateDate { get; set; }
    public int? Status { get; set; }
}