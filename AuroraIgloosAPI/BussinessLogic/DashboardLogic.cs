using System.Globalization;
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
            var previousTo = from.AddDays(-1);


            var currentBookings = _context.Booking
                .Where(b => b.LastModifiedAt >= from && b.LastModifiedAt <= to)
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
                
                if(!start.HasValue || !end.HasValue) return 0;
                var days = (end.Value.ToDateTime(TimeOnly.MinValue) - start.Value.ToDateTime(TimeOnly.MinValue)).Days;
                return days > 0 ? days : 0;
            });

            var occupancyPercent = maxOccupancy == 0 ? 0 : (double)occupancyCount / maxOccupancy * 100;

            var prevBookings = _context.Booking
                .Where(b => b.LastModifiedAt >= previousFrom && b.LastModifiedAt <= previousTo)
                .ToList();

            var prevCheckIns = prevBookings
                .Count(b => b.CheckIn <= previousTo);

            var prevOccupancyCount = prevBookings.Sum(b =>
            {
                var start = b.CheckIn < previousFrom ? previousFrom : b.CheckIn;
                var end = b.CheckOut > previousTo ? previousTo : b.CheckOut;
                if(!start.HasValue || !end.HasValue) return 0;
                var days = (end.Value.ToDateTime(TimeOnly.MinValue) - start.Value.ToDateTime(TimeOnly.MinValue)).Days;
                return days > 0 ? days : 0;
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

        public List<DashboardSalesPointDTO> GetSalesSeries(DateOnly from, DateOnly to)
        {
            var prevFrom = from.AddYears(-1);
            var prevTo   = to.AddYears(-1);

            // Pobieramy tylko te bookingi, które są potrzebne
            var all = _context.Booking
                .Where(b =>
                    (b.LastModifiedAt >= from && b.LastModifiedAt <= to) ||
                    (b.LastModifiedAt >= prevFrom && b.LastModifiedAt <= prevTo)
                )
                .Select(b => new { b.LastModifiedAt, b.Amount })
                .ToList();

            static string Key(DateOnly d) => $"{d.Year:D4}-{d.Month:D2}";

            var current = all
                .Where(x => x.LastModifiedAt >= from && x.LastModifiedAt <= to)
                .GroupBy(x => Key(x.LastModifiedAt))
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount)); // Amount jest decimal, nie nullable u Ciebie

            var previous = all
                .Where(x => x.LastModifiedAt >= prevFrom && x.LastModifiedAt <= prevTo)
                .GroupBy(x => Key(x.LastModifiedAt))
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

            var startMonth = new DateOnly(from.Year, from.Month, 1);
            var endMonth   = new DateOnly(to.Year, to.Month, 1);

            var months = new List<DateOnly>();
            for (var m = startMonth; m <= endMonth; m = m.AddMonths(1))
                months.Add(m);

            // static string MonthLabel(DateOnly d) => d.ToString("MMM"); 
            static string MonthLabel(DateOnly d) => d.ToString("MMM", CultureInfo.InvariantCulture);

            static string MonthKey(DateOnly d) => $"{d.Year:D4}-{d.Month:D2}";

            return months.Select(m =>
            {
                var kCurrent = MonthKey(m);
                var kPrev    = MonthKey(m.AddYears(-1)); // <<< klucz rok wcześniej

                return new DashboardSalesPointDTO
                {
                    Month = MonthLabel(m),
                    RevenueCurrentYear  = current.TryGetValue(kCurrent, out var c) ? c : 0m,
                    RevenuePreviousYear = previous.TryGetValue(kPrev, out var p) ? p : 0m
                };
            }).ToList();
        }

    }
}
