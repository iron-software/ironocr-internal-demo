using IronOcr;
using IronOCR_Demos.Demos;
using IronOCR_Demos.Demos.LanguagesGuide;

// ── License Setup ──────────────────────────────────────────────────────────
var licenseKey = Utils.GetLicence();
if (!string.IsNullOrWhiteSpace(licenseKey) && licenseKey != "YOUR-LICENSE-KEY-HERE")
{
    License.LicenseKey = licenseKey;
    Console.WriteLine("IronOCR license key loaded.");
}
else
{
    Console.WriteLine("WARNING: No valid license key found in appsettings.json.");
    Console.WriteLine("The demo will run in trial mode (watermarks may appear).");
    Console.WriteLine("See README.md for license key setup instructions.\n");
}

// ── Logging ────────────────────────────────────────────────────────────────
IronPdf.Logging.Logger.LogFilePath = "Default.log";
IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

// ── Demo Selection Menu ────────────────────────────────────────────────────
try
{
    while (true)
    {
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         IronOCR Demos                ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");
        Console.WriteLine("  1. Languages Guide - Multi-language PDF OCR");
        Console.WriteLine("  0. Exit\n");
        Console.Write("Select a demo [0-1]: ");

        var input = Console.ReadLine()?.Trim();

        switch (input)
        {
            case "1":
                var controller = new LanguagesGuideController();
                controller.Run();
                break;
            case "0":
                Console.WriteLine("Goodbye!");
                return;
            default:
                Console.WriteLine("Invalid selection. Please try again.");
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nAn error occurred: {ex.Message}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
