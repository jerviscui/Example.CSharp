using System.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace MiddlewareTest;

public class AnalysisDiagnosticAdapter
{
    private readonly ILogger<AnalysisDiagnosticAdapter> _logger;

    public AnalysisDiagnosticAdapter(ILogger<AnalysisDiagnosticAdapter> logger) => _logger = logger;

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareStarting")]
    public void OnMiddlewareStarting(string name, HttpContext httpContext, Guid instanceId, long timestamp)
    {
        _logger.LogInformation(
            $"{instanceId.ToString("N")} MiddlewareStarting: {name}, Path: {httpContext.Request.Path}, timestamp: {timestamp}");
    }

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareFinished")]
    public void OnMiddlewareFinished(string name, HttpContext httpContext, Guid instanceId, long timestamp,
        long duration)
    {
        _logger.LogInformation(
            $"{instanceId.ToString("N")} MiddlewareFinished: {name}, Path: {httpContext.Request.Path}, timestamp: {timestamp}, duration: {GetMillionsec(duration)}ms");
    }

    [DiagnosticName("Microsoft.AspNetCore.MiddlewareAnalysis.MiddlewareException")]
    public void OnMiddlewareException(string name, HttpContext httpContext, Guid instanceId, long timestamp,
        long duration, Exception ex)
    {
        _logger.LogInformation(
            $"{instanceId.ToString("N")} MiddlewareFinished: {name}, Path: {httpContext.Request.Path}, timestamp: {timestamp}, duration: {GetMillionsec(duration)}ms, ex: {ex.Message}");
    }

    private static long GetMillionsec(long duration)
    {
        return duration / (Stopwatch.Frequency / 1000);
    }
}
