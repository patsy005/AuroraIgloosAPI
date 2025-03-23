namespace AuroraIgloosAPI.DTOs
{
    public class IglooDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Capacity { get; set; }
        public decimal? PricePerNight { get; set; }

        public decimal Discount { get; set; }
        public required string DiscountName { get; set; }

    }
}
