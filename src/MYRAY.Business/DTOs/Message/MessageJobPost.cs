namespace MYRAY.Business.DTOs.Message;

public class MessageJobPost
{
    public int JobPostId { get; set; }
    public string JobPostTitle { get; set; }
    public List<Farmer> Farmers { get; set; }
}

public class MessageFarmer
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? PublishedId { get; set; }
    public string PublishedBy { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? LastMessageTime { get; set; }
}

public class Farmer
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Image { get; set; }
    public string ConventionId { get; set; }
    public MessageDetail LastMessage { get; set; }
}