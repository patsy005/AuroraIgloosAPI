namespace AuroraIgloosAPI.DTOs
{
    public class DashboardStatsDTO
    {
        public int Bookings { get; set; }
        public int CheckIns { get; set; }
        public double Occupancy { get; set; }
        public double BookingChangePercent { get; set; }
        public double CheckInChangePercent { get; set; }
        public double OccupancyChangePercent { get; set; }

    }
}
