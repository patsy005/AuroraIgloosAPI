namespace AuroraIgloosAPI.DTOs;

public class ReportRequestDTO
{
    public DateOnly From { get; set; }
    public DateOnly To { get; set; }

    public bool IncludeDashboard { get; set; } = true;
    public bool IncludeSales { get; set; } = true;
    public bool IncludeBookings { get; set; } = false;
    public bool IncludeIgloos  { get; set; } = false;
    public bool IncludeTrips { get; set; } = false;
    
    public string Format { get; set; } = "pdf";
}