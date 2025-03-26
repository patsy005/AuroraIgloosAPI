using AuroraIgloosAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace AuroraIgloosAPI.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "IdCustomer is required")]
        public int IdCustomer { get; set; }

        [Required(ErrorMessage = "IdIgloo is required")]
        public int IdIgloo { get; set; }

        [Required(ErrorMessage = "CheckIn is required")]
        public DateOnly CheckIn { get; set; }

        [Required(ErrorMessage = "CheckOut is required")]
        public DateOnly CheckOut { get; set; }

        [Required(ErrorMessage = "PaymentMethodId is required")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        //public int? StatusId { get; set; }

        [Required(ErrorMessage = "CreatedById is required")]
        public int CreatedById { get; set; }


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
