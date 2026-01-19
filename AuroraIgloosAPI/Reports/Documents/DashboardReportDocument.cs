using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Reports.Models;
using QuestPDF.Helpers;

namespace AuroraIgloosAPI.Reports.Documents
{
    public class DashboardReportDocument : IDocument
    {
        private readonly ReportData _data;
        private readonly ReportRequestDTO _request;

        public DashboardReportDocument(ReportData data, ReportRequestDTO request)
        {
            _data = data;
            _request = request;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        // ===== DARK THEME =====
        private static class Theme
        {
            // tło całej strony / główny bg
            public const string PageBg = "#001c1f";     // $primary-6
            public const string HeaderBg = "#003c43";   // $primary-13
            public const string CardBg = "#102e31";     // $overview-card
            public const string Border = "#2c8993";     // $primary-37
            public const string Accent = "#56efff";     // $primary-67

            public const string Text = "#cffaff";       // $primary-97
            public const string TextSoft = "#b8f8ff";   // $primary-86
            public const string Muted = "#c8c8c8";      // $grey-light

            // status / zmiany
            public const string Green = "#28be2e";      // $green-dark
            public const string Pink = "#d136af";       // $pink-dark
            public const string Orange = "#ffa24f";     // $orange-dark

            // tabelki
            public const string TableHeader = "#003c43";
            public const string RowAlt = "#0c2629";     // ciemniejsza zebra
            public const string Row = "#001c1f";        // jak tło strony
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(20);

                // Dark background całej strony
                page.PageColor(Theme.PageBg);

                // Domyślny styl tekstu (jasny!)
                page.DefaultTextStyle(t => t.FontSize(11).FontColor(Theme.Text));

                page.Header().Element(ComposeHeader);
                page.Content().PaddingTop(12).Element(ComposeContent);

                page.Footer()
                    .AlignCenter()
                    .Text($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm}")
                    .FontSize(9)
                    .FontColor(Theme.Muted);
            });
        }

        private void ComposeHeader(IContainer c)
        {
            c.Background(Theme.HeaderBg)
             .Padding(16)
             .CornerRadius(10)
             .Row(row =>
             {
                 row.RelativeItem().Column(col =>
                 {
                     col.Item().Text("Aurora Igloos – Business Report")
                         .FontSize(18)
                         .SemiBold()
                         .FontColor(Theme.Text);

                     col.Item().Text($"{_data.From:yyyy-MM-dd} → {_data.To:yyyy-MM-dd}")
                         .FontColor(Theme.TextSoft);
                 });

                 row.ConstantItem(120).AlignRight().AlignMiddle().Element(badge =>
                 {
                     badge.Background(Theme.Accent)
                          .PaddingVertical(6)
                          .PaddingHorizontal(12)
                          .CornerRadius(999)
                          .Text((_request.Format ?? "pdf").ToUpperInvariant())
                          .FontColor(Theme.PageBg)
                          .SemiBold();
                 });
             });
        }

        private void ComposeContent(IContainer c)
        {
            c.Column(col =>
            {
                col.Spacing(16);

                if (_request.IncludeDashboard && _data.DashboardStats != null)
                {
                    col.Item().Text("Dashboard Summary")
                        .FontSize(14).SemiBold().FontColor(Theme.TextSoft);

                    col.Item().Element(x => ComposeDashboardCards(x, _data.DashboardStats));
                }

                if (_request.IncludeSales && _data.Sales != null)
                {
                    col.Item().Text("Sales (Current vs Previous Year)")
                        .FontSize(14).SemiBold().FontColor(Theme.TextSoft);

                    col.Item().Element(x => ComposeSalesTable(x, _data.Sales));
                }

                if (_request.IncludeBookings && _data.Bookings != null)
                {
                    col.Item().Text("Bookings")
                        .FontSize(14).SemiBold().FontColor(Theme.TextSoft);

                    col.Item().Element(x => ComposeBookingsTable(x, _data.Bookings));
                }

                if (_request.IncludeIgloos && _data.Igloos != null)
                {
                    col.Item().Text("Igloos KPI")
                        .FontSize(14).SemiBold().FontColor(Theme.TextSoft);

                    col.Item().Element(x => ComposeIgloosTable(x, _data.Igloos));
                }

                if (_request.IncludeTrips && _data.Trips != null)
                {
                    col.Item().Text("Trips catalog")
                        .FontSize(14).SemiBold().FontColor(Theme.TextSoft);

                    col.Item().Element(x => ComposeTripsTable(x, _data.Trips));
                }
            });
        }

        // ===== DASHBOARD CARDS =====

        private void ComposeDashboardCards(IContainer c, DashboardStatsDTO s)
        {
            c.Row(row =>
            {
                row.Spacing(12);

                row.RelativeItem().Element(x => StatCard(x, "Bookings", s.Bookings.ToString(), s.BookingChangePercent));
                row.RelativeItem().Element(x => StatCard(x, "Check-ins", s.CheckIns.ToString(), s.CheckInChangePercent));
                row.RelativeItem().Element(x => StatCard(x, "Occupancy", $"{s.Occupancy:0.0}%", s.OccupancyChangePercent));
            });
        }

        private void StatCard(IContainer c, string title, string value, double changePercent)
        {
            var (badgeBg, badgeText) = ChangeBadge(changePercent);

            c.Background(Theme.CardBg)
             .Border(1).BorderColor(Theme.Border)
             .CornerRadius(10)
             .Padding(14)
             .Column(col =>
             {
                 col.Item().Text(title)
                     .FontColor(Theme.Muted);

                 col.Item().Row(r =>
                 {
                     r.RelativeItem().Text(value)
                         .FontSize(22)
                         .Bold()
                         .FontColor(Theme.Text);

                     r.ConstantItem(92).AlignRight().AlignMiddle().Element(badge =>
                     {
                         badge.Background(badgeBg)
                              .PaddingVertical(4)
                              .PaddingHorizontal(10)
                              .CornerRadius(999)
                              .Text(badgeText)
                              .FontSize(10)
                              .SemiBold()
                              .FontColor(Theme.PageBg);
                     });
                 });

                 col.Item().Text("vs previous period")
                     .FontSize(9)
                     .FontColor(Theme.Muted);
             });
        }

        private (string bg, string label) ChangeBadge(double change)
        {
            if (change > 0) return (Theme.Green, $"▲ {change:0.0}%");
            if (change < 0) return (Theme.Pink, $"▼ {Math.Abs(change):0.0}%");
            return (Theme.Orange, "— 0.0%");
        }

        // ===== TABLES =====

        private void ComposeSalesTable(IContainer c, List<DashboardSalesPointDTO> sales)
        {
            Table(
                c,
                header: new[] { "Month", "Current Year", "Previous Year" },
                rows: sales.Select(x => new[]
                {
                    x.Month,
                    $"{x.RevenueCurrentYear:0.00}",
                    $"{x.RevenuePreviousYear:0.00}"
                }).ToList()
            );
        }

        private void ComposeBookingsTable(IContainer c, List<BookingRowDTO> rows)
        {
            var show = rows.Take(45).ToList();

            Table(
                c,
                header: new[] { "ID", "Booking date", "Customer", "Igloo", "Trip", "Amount" },
                rows: show.Select(r => new[]
                {
                    r.BookingId.ToString(),
                    r.LastModifiedAt.ToString("yyyy-MM-dd"),
                    $"{r.CustomerName} {r.CustomerSurname}",
                    r.IglooName ?? "",
                    r.TripName ?? "",
                    $"{r.Amount:0.00}"
                }).ToList()
            );

            if (rows.Count > show.Count)
            {
                c.PaddingTop(6).Text($"Showing {show.Count} of {rows.Count} rows (full list in Excel).")
                    .FontSize(9)
                    .FontColor(Theme.Muted);
            }
        }

        private void ComposeIgloosTable(IContainer c, List<IglooRowDTO> rows)
        {
            Table(
                c,
                header: new[] { "Igloo", "Capacity", "Bookings", "Revenue", "Occupancy %" },
                rows: rows.Select(r => new[]
                {
                    r.Name,
                    r.Capacity.ToString(),
                    r.BookingsCount.ToString(),
                    $"{r.TotalRevenue:0.00}",
                    $"{r.OccupancyPercent:0.0}"
                }).ToList()
            );
        }

        private void ComposeTripsTable(IContainer c, List<TripRowDTO> rows)
        {
            Table(
                c,
                header: new[] { "Trip", "Duration", "Price/Person", "Difficulty", "Season" },
                rows: rows.Select(r => new[]
                {
                    r.Name,
                    r.Duration.ToString(),
                    $"{r.PricePerPerson:0.00}",
                    r.LevelOfDifficultyName,
                    r.SeasonName
                }).ToList()
            );
        }

        /// <summary>
        /// Dark table: header w primary-13, wiersze zebra, tekst jasny.
        /// </summary>
        private void Table(IContainer c, string[] header, List<string[]> rows)
        {
            c.Background(Theme.CardBg)
             .Border(1).BorderColor(Theme.Border)
             .CornerRadius(10)
             .Padding(10)
             .Table(t =>
             {
                 t.ColumnsDefinition(cols =>
                 {
                     for (int i = 0; i < header.Length; i++)
                         cols.RelativeColumn();
                 });

                 // Header
                 t.Header(h =>
                 {
                     foreach (var title in header)
                     {
                         h.Cell().Element(cell =>
                             cell.Background(Theme.TableHeader)
                                 .Padding(8)
                                 .BorderBottom(1)
                                 .BorderColor(Theme.Border)
                         ).Text(title)
                          .FontColor(Theme.Text)
                          .SemiBold();
                     }
                 });

                 // Body
                 for (int i = 0; i < rows.Count; i++)
                 {
                     var isAlt = i % 2 == 1;
                     var bg = isAlt ? Theme.RowAlt : Theme.Row;

                     foreach (var value in rows[i])
                     {
                         t.Cell().Element(cell =>
                             cell.Background(bg)
                                 .Padding(8)
                                 .BorderBottom(1)
                                 .BorderColor("#0f3b40")
                         ).Text(value ?? "")
                          .FontColor(Theme.TextSoft);
                     }
                 }
             });
        }
    }
}

