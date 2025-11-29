using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class ForumPostDTO
    {
        public int Id { get; set; }
        public required int IdEmployee { get; set; }
        public required int CategoryId { get; set; }
        public required string Title { get; set; }
        public required string PostContent { get; set; }
        public  DateOnly? PostDate { get; set; }
        public string? Category { get; set; }
        public string? Tags { get; set; }

        public int NumberOfComment { get; set; }
        
        public Employee Employee { get; set; }

        public ICollection<ForumCommentDTO>? ForumComment { get; set; }
    }
}
