using AuroraIgloosAPI.Models;
using AuroraIgloosAPI.Models.Contexts;
using Microsoft.EntityFrameworkCore;


namespace AuroraIgloosAPI.BussinessLogic
{
    public class BookingsLogic
    {
        private readonly CompanyContext _context;

        public BookingsLogic(CompanyContext context)
        {
            _context = context;
        }

        public decimal? CalculateBookingTotalAmount(int? idIgloo, DateOnly? checkIn, DateOnly? checkOut, DateOnly? bookingDate)
        {
           
            if (!idIgloo.HasValue || !checkIn.HasValue || !checkOut.HasValue) return null;
            
            var igloo = _context.Igloo
                .Include(i => i.Discount)
                .FirstOrDefault(i => i.Id == idIgloo);

            if (igloo == null || !igloo.PricePerNight.HasValue) return null;
            
            var days = (checkOut.Value.ToDateTime(TimeOnly.MinValue) - checkIn.Value.ToDateTime(TimeOnly.MinValue)).TotalDays;
            
            if (days <= 0) return null;

            var discount = igloo.Discount;

            decimal? totalAmount = 0;

            if (discount != null && discount.Discount1 != 0 && bookingDate.HasValue)
            {
                bool fromOk = !discount.ValidFrom.HasValue || bookingDate >= discount.ValidFrom;
                bool toOk = !discount.ValidTo.HasValue || bookingDate <= discount.ValidTo;

                if (fromOk && toOk)
                {
                    totalAmount = igloo.PricePerNight * (decimal)days * (1 - igloo.Discount.Discount1 / 100);
                }
            }
            else
            {
                totalAmount = igloo.PricePerNight * (decimal)days;
            }
            
            return Math.Round(totalAmount.Value, 2);

        }

        public decimal? CalculateTripAmount(int? tripId, int guests)
        {
            if(!tripId.HasValue || guests <= 0) return null;
            
            var trip = _context.Trip.FirstOrDefault(t => t.Id == tripId);
            
            if(trip == null) return null;
            
            var total = trip.PricePerPerson * guests;
            
            return Math.Round(total, 2);
        }

    }
}
