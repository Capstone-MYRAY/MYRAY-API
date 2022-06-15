using System.ComponentModel;

namespace MYRAY.Business.DTOs.Comment;

public class SearchComment
{
    [DefaultValue(null)]public int? CommentBy { get; set; }= null;
    [DefaultValue("")]public string? Content { get; set; }= "";
}