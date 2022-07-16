namespace MYRAY.Business.DTOs.Message;

public class MessageJobPost
{
    public int JobPostId { get; set; }
    public string JobPostTitle { get; set; }
    public List<ListFarmer> Farmers { get; set; }
}

public class ListFarmer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public MessageDetail LastMessage { get; set; }
}