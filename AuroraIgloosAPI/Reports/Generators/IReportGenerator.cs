using AuroraIgloosAPI.DTOs;
using AuroraIgloosAPI.Reports.Models;

namespace AuroraIgloosAPI.Reports.Generators;

public interface IReportGenerator
{
    string ContentType { get; }
    string FileExtension { get; }

    byte[] Generate(ReportData data, ReportRequestDTO request);
}