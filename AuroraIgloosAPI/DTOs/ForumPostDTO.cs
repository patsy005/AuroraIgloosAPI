using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class ForumPostDTO
    {
        public int Id { get; set; }
        public int? IdEmployee { get; set; }
        public int? CategoryId { get; set; }
        public required string Title { get; set; }
        public required string PostContent { get; set; }
        public  DateOnly? PostDate { get; set; }
        public required string Category { get; set; }
        public required string Tags { get; set; }

        public required string EmployeeName {get; set;}
        public required string EmployeeSurname { get; set; }

        public required string EmployeePhotoUrl { get; set; }

        public int NumberOfComment { get; set; }

        public ICollection<ForumCommentDTO>? ForumComment { get; set; }
    }
}
