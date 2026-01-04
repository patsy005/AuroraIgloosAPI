using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.BussinessLogic;
using AuroraIgloosAPI.Models.Contexts;
using AuroraIgloosAPI.Reports.Generators;
using AuroraIgloosAPI.Reports.Models;

namespace AuroraIgloosAPI.Reports;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly CompanyContext _context;
    private readonly DashboardLogic _dashboardLogic;
    private readonly IEnumerable<IReportGenerator> _generators;

    public ReportsController(
        CompanyContext context,
        DashboardLogic dashboardLogic,
        IEnumerable<IReportGenerator> generators)
    {
        _context = context;
        _dashboardLogic = dashboardLogic;
        _generators = generators;
    }

    [HttpPost("dashboard")]
    public async Task<IActionResult> Generate([FromBody] ReportRequestDTO req)
    {
        var data = new ReportData
        {
            From = req.From,
            To = req.To,

            DashboardStats = req.IncludeDashboard
                ? _dashboardLogic.GetDashboardStats(req.From, req.To)
                : null,

            Sales = req.IncludeSales
                ? _dashboardLogic.GetSalesSeries(req.From, req.To)
                : null,

            Bookings = req.IncludeBookings
                ? await GetBookings(req.From, req.To)
                : null,

            Igloos = req.IncludeIgloos
                ? await GetIgloos(req.From, req.To)
                : null,

            Trips = req.IncludeTrips
                ? await GetTrips()
                : null
        };

        var format = (req.Format ?? "pdf").ToLowerInvariant();
        var generator = format switch
        {
            "xlsx" or "excel" => _generators.First(g => g.FileExtension == "xlsx"),
            _ => _generators.First(g => g.FileExtension == "pdf"),
        };

        var bytes = generator.Generate(data, req);
        var fileName = $"report_{req.From:yyyyMMdd}_{req.To:yyyyMMdd}.{generator.FileExtension}";
        return File(bytes, generator.ContentType, fileName);
    }

    // ===================== DATA LOADERS =====================

    private async Task<List<BookingRowDTO>> GetBookings(DateOnly from, DateOnly to)
    {
        //  modele:
        // Booking.Customer.Person
        // Booking.PaymentMethod
        // Booking.Igloo
        // Booking.Trip
        return await _context.Booking
            .Where(b => b.BookingDate >= from && b.BookingDate <= to)
            .Include(b => b.Customer).ThenInclude(c => c.Person)
            .Include(b => b.PaymentMethod)
            .Include(b => b.Igloo)
            .Include(b => b.Trip)
            .Select(b => new BookingRowDTO
            {
                BookingId = b.Id,

                CustomerName = b.Customer.Person.Name,
                CustomerSurname = b.Customer.Person.Surname,
                CustomerEmail = b.Customer.Person.Email,

                IglooName = b.Igloo != null ? (b.Igloo.Name ?? "") : "",

                CheckIn = b.CheckIn,
                CheckOut = b.CheckOut,

                BookingDate = b.BookingDate,

                TripDate = b.TripDate,
                TripName = b.Trip != null ? b.Trip.Name : "",

                Amount = b.Amount,
                Guests = b.Guests,

                PaymentMethodName = b.PaymentMethod.Name,

                EarlyCheckInRequest = b.EarlyCheckInRequest == true ? "YES" : "NO",
                LateCheckOutRequest = b.LateCheckOutRequest == true ? "YES" : "NO",
            })
            .ToListAsync();
    }

    private async Task<List<IglooRowDTO>> GetIgloos(DateOnly from, DateOnly to)
    {
        var igloos = await _context.Igloo
            .Include(i => i.Discount)
            .ToListAsync();

        // Booking może mieć igloo albo nie.
        // Bierzemy TYLKO te bookingi, które mają igloo i mają daty pobytu,
        // i które nachodzą na okres [from,to]
        var bookings = await _context.Booking
            .Where(b =>
                b.IdIgloo.HasValue &&
                b.CheckIn.HasValue &&
                b.CheckOut.HasValue &&
                b.CheckIn.Value <= to &&
                b.CheckOut.Value >= from
            )
            .ToListAsync();

        var totalDays = (to.DayNumber - from.DayNumber) + 1;

        return igloos.Select(i =>
        {
            var bForIgloo = bookings.Where(b => b.IdIgloo == i.Id).ToList();

            var bookingsCount = bForIgloo.Count;

            // Proste revenue: suma Amount z bookingów igloo.
            // Jeśli Amount u Ciebie zawiera też trip, to można zmienić na (PricePerNight * occupiedDays).
            var totalRevenue = bForIgloo.Sum(b => b.Amount);

            var occupiedDays = bForIgloo.Sum(b =>
            {
                var start = b.CheckIn!.Value < from ? from : b.CheckIn!.Value;
                var end = b.CheckOut!.Value > to ? to : b.CheckOut!.Value;

                var days = (end.ToDateTime(TimeOnly.MinValue) - start.ToDateTime(TimeOnly.MinValue)).Days;
                return days > 0 ? days : 0;
            });

            var maxDays = totalDays; // 1 igloo * dni w okresie
            var occ = maxDays == 0 ? 0 : (double)occupiedDays / maxDays * 100;

            return new IglooRowDTO
            {
                IglooId = i.Id,
                Name = i.Name ?? "",
                Capacity = i.Capacity ?? 0,
                PricePerNight = (int)Math.Round(i.PricePerNight ?? 0m),

                Discount = i.Discount,
                Description = i.Description,

                BookingsCount = bookingsCount,
                TotalRevenue = totalRevenue,
                OccupancyPercent = Math.Round(occ, 1),
            };
        }).ToList();
    }

    private async Task<List<TripRowDTO>> GetTrips()
    {
        return await _context.Trip
            .Include(t => t.LevelOfDifficulty)
            .Include(t => t.Season)
            .Select(t => new TripRowDTO
            {
                TripId = t.Id,
                Name = t.Name,
                Duration = t.Duration,
                PricePerPerson = t.PricePerPerson,
                ShortDescription = t.ShortDescription,
                LongDescription = t.LongDescription,
                LevelOfDifficultyName = t.LevelOfDifficulty != null ? t.LevelOfDifficulty.Name : "",
                SeasonName = t.Season != null ? t.Season.Name : "",
            })
            .ToListAsync();
    }
}
