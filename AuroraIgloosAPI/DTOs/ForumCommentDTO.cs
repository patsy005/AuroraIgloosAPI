namespace AuroraIgloosAPI.DTOs
{
    public class ForumCommentDTO
    {
        public int? Id { get; set; }
        public int? IdPost { get; set; }
        public int? IdEmployee { get; set; }
        public string? Comment { get; set; }
        public DateOnly? CommentDate { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }
        public string? EmployeePhotoUrl { get; set; }

        public string? PostTitle { get; set; }
    }
}
