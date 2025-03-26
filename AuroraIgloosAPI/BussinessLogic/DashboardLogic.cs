using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Models.Contexts;

namespace AuroraIgloosAPI.BussinessLogic
{
    public class DashboardLogic
    {
        private readonly CompanyContext _context;

        public DashboardLogic(CompanyContext context)
        {
            _context = context;
        }

        public DashboardStatsDTO GetDashboardStats(DateOnly from, DateOnly to)
        {
            var previousFrom = from.AddDays(-(to.DayNumber - from.DayNumber));
            var previousTo = from;

            var currentBookings = _context.Booking
                .Where(b => b.BookingDate >= from && b.BookingDate <= to)
                .ToList();

            var currentCheckIns = currentBookings
                .Count(b => b.CheckIn <= DateOnly.FromDateTime(DateTime.Now));

            var totalDays = (to.DayNumber - from.DayNumber) + 1;
            var totalIgloos = _context.Igloo.Count();
            var maxOccupancy = totalIgloos * totalDays;

            var occupancyCount = currentBookings.Sum(b =>
            {
                var start = b.CheckIn < from ? from : b.CheckIn;
                var end = b.CheckOut > to ? to : b.CheckOut;
                var days = end.DayNumber - start.DayNumber;
                return days > 0 ? days : 0;
            });

            var occupancyPercent = maxOccupancy == 0 ? 0 : (double)occupancyCount / maxOccupancy * 100;

            var prevBookings = _context.Booking
                .Where(b => b.BookingDate >= previousFrom && b.BookingDate <= previousTo)
                .ToList();

            var prevCheckIns = prevBookings
                .Count(b => b.CheckIn <= previousTo);

            var prevOccupancyCount = prevBookings.Sum(b =>
            {
                var start = b.CheckIn < previousFrom ? previousFrom : b.CheckIn;
                var end = b.CheckOut > previousTo ? previousTo : b.CheckOut;
                return (end.DayNumber - start.DayNumber);
            });

            var previousMaxOccupancy = totalDays * totalIgloos;
            var previousOccupancyPercent = previousMaxOccupancy == 0 ? 0 : (double)prevOccupancyCount / previousMaxOccupancy * 100;

            // % zmiany
            double GetChangePercent(double current, double previous)
            {
                if (previous == 0) return current == 0 ? 0 : 100;
                return Math.Round(((current - previous) / previous) * 100, 1);
            }

            return new DashboardStatsDTO
            {
                Bookings = currentBookings.Count,
                CheckIns = currentCheckIns,
                Occupancy = Math.Round(occupancyPercent, 1),
                BookingChangePercent = GetChangePercent(currentBookings.Count, prevBookings.Count),
                CheckInChangePercent = GetChangePercent(currentCheckIns, prevCheckIns),
                OccupancyChangePercent = GetChangePercent(occupancyPercent, previousOccupancyPercent)
            };
        }
    }
}
