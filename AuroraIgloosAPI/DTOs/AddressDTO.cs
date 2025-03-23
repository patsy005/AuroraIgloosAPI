namespace AuroraIgloosAPI.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public string? HouseNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
    }
}
