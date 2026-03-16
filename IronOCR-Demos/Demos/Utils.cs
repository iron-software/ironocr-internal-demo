using Microsoft.Extensions.Configuration;

namespace IronOCR_Demos.Demos;

/// <summary>
/// Shared utilities for all demos — license management and common helpers.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Retrieves the IronOCR/IronPDF license key.
    /// REPLACE WITH YOUR OWN LICENCE KEY or set in appsettings.json
    /// </summary>
    public static string GetLicence()
    {
        return GetLicenseFromConfig();
    }

    /// <summary>
    /// Reads the license key from appsettings.json in the project root.
    /// </summary>
    private static string GetLicenseFromConfig()
    {
        // Navigate up from bin/Debug/net9.0 to project root
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var config = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        return config["IronOcr.LicenseKey"] ?? "";
    }

    /// <summary>
    /// Resolves a path relative to the project root (3 levels up from bin output).
    /// </summary>
    public static string GetProjectRootPath(params string[] pathSegments)
    {
        var basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        return Path.Combine(new[] { basePath }.Concat(pathSegments).ToArray());
    }
}
