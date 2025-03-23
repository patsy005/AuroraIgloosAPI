using AuroraIgloosAPI.Models;

namespace AuroraIgloosAPI.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int? IdCustomer { get; set; }
        public int? IdIgloo { get; set; }
        public DateOnly? CheckIn { get; set; }
        public DateOnly? CheckOut { get; set; }
        public int? PaymentMethodId { get; set; }
        public decimal? Amount { get; set; }
        public int? StatusId { get; set; }
        public int? CreatedById { get; set; }
        public bool? EarlyCheckInRequest { get; set; }
        public bool? LateCheckOutRequest { get; set; }
        public DateOnly? BookingDate { get; set; }

        //public Customer? Customer { get; set; }
        //public Igloo? Igloo { get; set; }
        //public Employee? Employee { get; set; }
        //public PaymentMethod? PaymentMethod { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerSurname { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }

        public string? IglooName { get; set; }

        public string? EmployeeName { get; set; }
        public string? EmployeeSurname { get; set; }

        public string? PaymentMethodName { get; set; }


    }
}
