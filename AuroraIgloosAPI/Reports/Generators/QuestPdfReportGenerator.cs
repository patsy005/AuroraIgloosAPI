using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Reports.Documents;
using AuroraIgloosAPI.Reports.Models;
using QuestPDF.Fluent;

namespace AuroraIgloosAPI.Reports.Generators;

public class QuestPdfReportGenerator : IReportGenerator
{
    public string ContentType => "application/pdf";
    public string FileExtension => "pdf";

    public byte[] Generate(ReportData data, ReportRequestDTO request)
    {
        var doc = new DashboardReportDocument(data, request);
        return doc.GeneratePdf();
    }
}