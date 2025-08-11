namespace XTAReportingEngine;

public static class XReportingConsts
{
    // Absolute path to XTAReportingEngine\XTAReports relative to the engine's project root
    public static readonly string XTA_REPORTING_DIR = ResolveReportRoot();

    private static string ResolveReportRoot()
    {
        // Try to locate the project root by walking up until a marker file is found
        string? current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            var csproj = Path.Combine(current, "XTAReportingEngine.csproj");
            var reportsDir = Path.Combine(current, "XTAReports");
            var sln = Path.Combine(current, "XTA.sln");

            if (File.Exists(csproj) || Directory.Exists(reportsDir) || File.Exists(sln))
            {
                return Directory.Exists(reportsDir) ? reportsDir : Path.Combine(current, "XTAReports");
            }

            current = Directory.GetParent(current)?.FullName;
        }

        // Fallback: keep artifacts near the executable
        return Path.Combine(AppContext.BaseDirectory, "report");
    }
}


