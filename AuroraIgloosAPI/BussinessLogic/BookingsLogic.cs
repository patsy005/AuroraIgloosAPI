using AuroraIgloosAPI.Models.Contexts;

namespace AuroraIgloosAPI.BussinessLogic
{
    public class BookingsLogic
    {
        private readonly CompanyContext _context;

        public BookingsLogic(CompanyContext context)
        {
            _context = context;
        }

        public decimal? CalculateBookingTotalAmount(int? idIgloo, DateOnly checkIn, DateOnly checkOut)
        {
            var discount = _context.Discount.FirstOrDefault(d => d.IdIgloo == idIgloo);

            decimal? totalAmount = 0;
            var igloo = _context.Igloo.Find(idIgloo);

            if (igloo != null)
            {
                var days = (checkOut.ToDateTime(TimeOnly.MinValue) - checkIn.ToDateTime(TimeOnly.MinValue)).TotalDays;

                if(discount != null && discount.Discount1 != 0)
                {
                    totalAmount = igloo.PricePerNight * (decimal)days * (1 - discount.Discount1 / 100);
                }
                else
                {
                    totalAmount = igloo.PricePerNight * (decimal)days;
                }
            }

            return Math.Round(totalAmount.Value, 2);
        }
    }
}
