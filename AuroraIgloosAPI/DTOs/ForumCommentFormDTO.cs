using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs;

public class ForumCommentFormDTO
{
    public int Id { get; set; }

    public int IdPost { get; set; }
    public int IdEmployee { get; set; }
    public string Comment { get; set; }
    public DateOnly? CommentDate { get; set; }
    
    public DateOnly? UpdateDate { get; set; }

    public Employee Employee { get; set; }

    public ForumPost ForumPost { get; set; }
}