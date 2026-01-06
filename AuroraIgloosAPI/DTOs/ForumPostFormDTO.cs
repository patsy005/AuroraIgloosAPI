namespace AuroraIgloosAPI.DTOs;

public class ForumPostFormDTO
{
    public int Id { get; set; }
    public int IdEmployee { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string PostContent { get; set; }

    public string Tags { get; set; }
    
    public  DateOnly? PostDate { get; set; }
    
    public ICollection<ForumCommentDTO>? ForumComment { get; set; }
    
}