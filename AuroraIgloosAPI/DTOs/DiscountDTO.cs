namespace AuroraIgloosAPI.DTOs
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public int? IdIgloo { get; set; }
        public string? Name { get; set; }
        public decimal Discount { get; set; }
        public string? Description { get; set; }

        public required string IglooName { get; set; }
    }
}
