using AuroraIgloosAPI.DTOs;

namespace AuroraIgloosAPI.Reports.Models;

public class ReportData
{
    // zbiera dane potrzebne do wygenerowania PDF/Excel (paczka danych)
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }
    
    public DashboardStatsDTO? DashboardStats { get; set; }
    public List<DashboardSalesPointDTO>? Sales { get; set; }
    
    public List<BookingRowDTO>? Bookings { get; set; }
    public List<IglooRowDTO>? Igloos { get; set; }
    public List<TripRowDTO>? Trips { get; set; }
    
}