using ClosedXML.Excel;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Reports.Models;

namespace AuroraIgloosAPI.Reports.Generators
{
    public class ClosedXmlReportGenerator : IReportGenerator
    {
        public string ContentType =>
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public string FileExtension => "xlsx";

        public byte[] Generate(ReportData data, ReportRequestDTO request)
        {
            using var wb = new XLWorkbook();

            if (request.IncludeDashboard && data.DashboardStats != null)
                AddDashboardSheet(wb, data);

            if (request.IncludeSales && data.Sales != null)
                AddSalesSheet(wb, data.Sales);

            if (request.IncludeBookings && data.Bookings != null)
                AddBookingsSheet(wb, data.Bookings);

            if (request.IncludeIgloos && data.Igloos != null)
                AddIgloosSheet(wb, data.Igloos);

            if (request.IncludeTrips && data.Trips != null)
                AddTripsSheet(wb, data.Trips);

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        private static void AddDashboardSheet(XLWorkbook wb, ReportData data)
        {
            var s = data.DashboardStats!;
            var ws = wb.Worksheets.Add("Dashboard");

            ws.Cell(1, 1).Value = "From";
            ws.Cell(1, 2).Value = data.From.ToString("yyyy-MM-dd");
            ws.Cell(2, 1).Value = "To";
            ws.Cell(2, 2).Value = data.To.ToString("yyyy-MM-dd");

            ws.Cell(4, 1).Value = "Bookings";
            ws.Cell(4, 2).Value = s.Bookings;

            ws.Cell(5, 1).Value = "Check-ins";
            ws.Cell(5, 2).Value = s.CheckIns;

            ws.Cell(6, 1).Value = "Occupancy (%)";
            ws.Cell(6, 2).Value = s.Occupancy;

            ws.Columns().AdjustToContents();
        }

        private static void AddSalesSheet(XLWorkbook wb, List<DashboardSalesPointDTO> sales)
        {
            var ws = wb.Worksheets.Add("Sales");

            ws.Cell(1, 1).Value = "Month";
            ws.Cell(1, 2).Value = "Revenue (Current)";
            ws.Cell(1, 3).Value = "Revenue (Previous)";

            for (int i = 0; i < sales.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = sales[i].Month;
                ws.Cell(i + 2, 2).Value = sales[i].RevenueCurrentYear;
                ws.Cell(i + 2, 3).Value = sales[i].RevenuePreviousYear;
            }

            ws.Columns().AdjustToContents();
        }

        private static void AddBookingsSheet(XLWorkbook wb, List<BookingRowDTO> rows)
        {
            var ws = wb.Worksheets.Add("Bookings");

            ws.Cell(1, 1).Value = "BookingId";
            ws.Cell(1, 2).Value = "BookingDate";
            ws.Cell(1, 3).Value = "CustomerName";
            ws.Cell(1, 4).Value = "CustomerSurname";
            ws.Cell(1, 5).Value = "CustomerEmail";
            ws.Cell(1, 6).Value = "IglooName";
            ws.Cell(1, 7).Value = "CheckIn";
            ws.Cell(1, 8).Value = "CheckOut";
            ws.Cell(1, 9).Value = "TripName";
            ws.Cell(1,10).Value = "TripDate";
            ws.Cell(1,11).Value = "Guests";
            ws.Cell(1,12).Value = "Amount";
            ws.Cell(1,13).Value = "PaymentMethod";
            ws.Cell(1,14).Value = "EarlyCheckIn";
            ws.Cell(1,15).Value = "LateCheckOut";

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var row = i + 2;

                ws.Cell(row, 1).Value = r.BookingId;
                ws.Cell(row, 2).Value = r.BookingDate.ToString("yyyy-MM-dd");
                ws.Cell(row, 3).Value = r.CustomerName;
                ws.Cell(row, 4).Value = r.CustomerSurname;
                ws.Cell(row, 5).Value = r.CustomerEmail;
                ws.Cell(row, 6).Value = r.IglooName;
                ws.Cell(row, 7).Value = r.CheckIn?.ToString("yyyy-MM-dd") ?? "";
                ws.Cell(row, 8).Value = r.CheckOut?.ToString("yyyy-MM-dd") ?? "";
                ws.Cell(row, 9).Value = r.TripName;
                ws.Cell(row,10).Value = r.TripDate?.ToString("yyyy-MM-dd") ?? "";
                ws.Cell(row,11).Value = r.Guests;
                ws.Cell(row,12).Value = r.Amount;
                ws.Cell(row,13).Value = r.PaymentMethodName;
                ws.Cell(row,14).Value = r.EarlyCheckInRequest;
                ws.Cell(row,15).Value = r.LateCheckOutRequest;
            }

            ws.Columns().AdjustToContents();
        }

        private static void AddIgloosSheet(XLWorkbook wb, List<IglooRowDTO> rows)
        {
            var ws = wb.Worksheets.Add("Igloos");

            ws.Cell(1, 1).Value = "IglooId";
            ws.Cell(1, 2).Value = "Name";
            ws.Cell(1, 3).Value = "Capacity";
            ws.Cell(1, 4).Value = "PricePerNight";
            ws.Cell(1, 5).Value = "Discount";
            ws.Cell(1, 6).Value = "BookingsCount";
            ws.Cell(1, 7).Value = "TotalRevenue";
            ws.Cell(1, 8).Value = "OccupancyPercent";
            ws.Cell(1, 9).Value = "Description";

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var row = i + 2;

                ws.Cell(row, 1).Value = r.IglooId;
                ws.Cell(row, 2).Value = r.Name;
                ws.Cell(row, 3).Value = r.Capacity;
                ws.Cell(row, 4).Value = r.PricePerNight;

                ws.Cell(row, 5).Value = r.Discount != null ? r.Discount.ToString() : "";

                ws.Cell(row, 6).Value = r.BookingsCount;
                ws.Cell(row, 7).Value = r.TotalRevenue;
                ws.Cell(row, 8).Value = r.OccupancyPercent;
                ws.Cell(row, 9).Value = r.Description ?? "";
            }

            ws.Columns().AdjustToContents();
        }

        private static void AddTripsSheet(XLWorkbook wb, List<TripRowDTO> rows)
        {
            var ws = wb.Worksheets.Add("Trips");

            ws.Cell(1, 1).Value = "TripId";
            ws.Cell(1, 2).Value = "Name";
            ws.Cell(1, 3).Value = "Duration";
            ws.Cell(1, 4).Value = "PricePerPerson";
            ws.Cell(1, 5).Value = "ShortDescription";
            ws.Cell(1, 6).Value = "LongDescription";
            ws.Cell(1, 7).Value = "LevelOfDifficulty";
            ws.Cell(1, 8).Value = "Season";

            for (int i = 0; i < rows.Count; i++)
            {
                var r = rows[i];
                var row = i + 2;

                ws.Cell(row, 1).Value = r.TripId;
                ws.Cell(row, 2).Value = r.Name;
                ws.Cell(row, 3).Value = r.Duration;
                ws.Cell(row, 4).Value = r.PricePerPerson;
                ws.Cell(row, 5).Value = r.ShortDescription ?? "";
                ws.Cell(row, 6).Value = r.LongDescription ?? "";
                ws.Cell(row, 7).Value = r.LevelOfDifficultyName;
                ws.Cell(row, 8).Value = r.SeasonName;
            }

            ws.Columns().AdjustToContents();
        }
    }
}
