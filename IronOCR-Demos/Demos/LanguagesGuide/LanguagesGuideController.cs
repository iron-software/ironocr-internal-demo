using System.Diagnostics;
using System.Text;

namespace IronOCR_Demos.Demos.LanguagesGuide;

/// <summary>
/// CLI controller for the Languages Guide demo.
/// Handles scenario selection, runs OCR, and generates an HTML comparison report.
/// </summary>
public class LanguagesGuideController
{
    private readonly LanguagesGuideDemo _demo = new();
    private readonly string _examplesDir;
    private readonly string _expectedTextDir;
    private readonly string _outputDir;

    // Scenario definitions
    private readonly List<(string Key, string Label, string Description)> _scenarios = new()
    {
        ("english",        "English",            "Latin script — baseline"),
        ("spanish",        "Spanish",            "Latin script — accented characters"),
        ("chinese",        "Chinese Simplified", "CJK characters"),
        ("russian",        "Russian",            "Cyrillic script"),
        ("multi-language", "Multi-Language",      "Mixed scripts — flagship demo"),
    };

    public LanguagesGuideController()
    {
        // Get the project root directory (3 levels up from bin\Debug\net9.0)
        string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\"));
        _examplesDir = Path.Combine(projectRoot, "Demos", "LanguagesGuide", "Examples");
        _expectedTextDir = Path.Combine(projectRoot, "Demos", "LanguagesGuide", "ExpectedText");
        _outputDir = Path.Combine(projectRoot, "Output");
        Directory.CreateDirectory(_outputDir);
    }

    /// <summary>
    /// Main entry point — displays the scenario menu and runs selected scenarios.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════════════╗");
            Console.WriteLine("║       Languages Guide                ║");
            Console.WriteLine("╚══════════════════════════════════════╝\n");

            for (int i = 0; i < _scenarios.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {_scenarios[i].Label,-25} ({_scenarios[i].Description})");
            }
            Console.WriteLine($"  {_scenarios.Count + 1}. Run all scenarios");
            Console.WriteLine($"  0. Back to main menu\n");
            Console.Write($"Select [0-{_scenarios.Count + 1}]: ");

            var input = Console.ReadLine()?.Trim();

            if (input == "0") return;

            if (input == $"{_scenarios.Count + 1}")
            {
                RunAllScenarios();
                continue;
            }

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= _scenarios.Count)
            {
                var results = new List<(LanguagesGuideDemo.ScenarioResult Result, string ExpectedText, double Accuracy)>();
                var scenarioResult = RunSingleScenario(choice - 1);
                if (scenarioResult != null)
                {
                    results.Add(scenarioResult.Value);
                    GenerateHtmlReport(results);
                }
            }
            else
            {
                Console.WriteLine("Invalid selection. Please try again.");
            }
        }
    }

    private void RunAllScenarios()
    {
        Console.WriteLine("\nRunning all scenarios...\n");
        var results = new List<(LanguagesGuideDemo.ScenarioResult Result, string ExpectedText, double Accuracy)>();

        for (int i = 0; i < _scenarios.Count; i++)
        {
            var result = RunSingleScenario(i);
            if (result != null)
            {
                results.Add(result.Value);
            }
        }

        if (results.Count > 0)
        {
            GenerateHtmlReport(results);
        }
    }

    private (LanguagesGuideDemo.ScenarioResult Result, string ExpectedText, double Accuracy)? RunSingleScenario(int index)
    {
        var scenario = _scenarios[index];
        var pdfPath = Path.Combine(_examplesDir, $"{scenario.Key}-sample.pdf");
        var expectedPath = Path.Combine(_expectedTextDir, $"{scenario.Key}-expected.txt");

        if (!File.Exists(pdfPath))
        {
            Console.WriteLine($"  ERROR: Sample PDF not found: {pdfPath}");
            Console.WriteLine("  Run the GenerateSamplePdfs tool first. See README.md for instructions.");
            return null;
        }

        Console.Write($"  Processing {scenario.Label}... ");

        try
        {
            var result = scenario.Key switch
            {
                "english" => _demo.ReadEnglish(pdfPath),
                "spanish" => _demo.ReadSpanish(pdfPath),
                "chinese" => _demo.ReadChinese(pdfPath),
                "russian" => _demo.ReadRussian(pdfPath),
                "multi-language" => _demo.ReadMultiLanguage(pdfPath),
                _ => throw new InvalidOperationException($"Unknown scenario: {scenario.Key}")
            };

            var expectedText = File.Exists(expectedPath) ? File.ReadAllText(expectedPath).Trim() : "(no expected text file)";
            var accuracy = TextComparer.CalculateAccuracy(result.ExtractedText, expectedText);

            Console.WriteLine($"Done ({result.ProcessingTime.TotalSeconds:F1}s, accuracy: {accuracy:F1}%)");

            return (result, expectedText, accuracy);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FAILED: {ex.Message}");
            return null;
        }
    }

    // ── HTML Report Generation ─────────────────────────────────────────────

    private void GenerateHtmlReport(List<(LanguagesGuideDemo.ScenarioResult Result, string ExpectedText, double Accuracy)> results)
    {
        var reportPath = Path.Combine(_outputDir, $"LanguagesGuide-Report-{DateTime.Now:yyyyMMdd-HHmmss}.html");
        var html = BuildHtmlReport(results);
        File.WriteAllText(reportPath, html);

        Console.WriteLine($"\n  Report generated: {reportPath}");

        // Try to open in default browser
        try
        {
            Process.Start(new ProcessStartInfo(reportPath) { UseShellExecute = true });
            Console.WriteLine("  Report opened in default browser.");
        }
        catch
        {
            Console.WriteLine("  (Could not open browser automatically — open the file manually)");
        }
    }

    private string BuildHtmlReport(List<(LanguagesGuideDemo.ScenarioResult Result, string ExpectedText, double Accuracy)> results)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang='en'>");
        sb.AppendLine("<head>");
        sb.AppendLine("  <meta charset='UTF-8'>");
        sb.AppendLine("  <title>IronOCR Languages Guide — Report</title>");
        sb.AppendLine("  <style>");
        sb.AppendLine(GetReportCss());
        sb.AppendLine("  </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");

        // Header
        sb.AppendLine("  <header>");
        sb.AppendLine("    <h1>IronOCR Languages Guide</h1>");
        sb.AppendLine($"    <p class='subtitle'>Multi-Language OCR Report — Generated {DateTime.Now:MMMM d, yyyy h:mm tt}</p>");
        sb.AppendLine("  </header>");

        // Summary table
        sb.AppendLine("  <section class='summary'>");
        sb.AppendLine("    <h2>Summary</h2>");
        sb.AppendLine("    <table>");
        sb.AppendLine("      <thead><tr><th>Language</th><th>Script</th><th>Accuracy</th><th>Engine Confidence</th><th>Time</th><th>Pages</th></tr></thead>");
        sb.AppendLine("      <tbody>");
        foreach (var (result, _, accuracy) in results)
        {
            var accuracyClass = accuracy >= 90 ? "high" : accuracy >= 70 ? "medium" : "low";
            sb.AppendLine($"        <tr>");
            sb.AppendLine($"          <td>{Encode(result.LanguageName)}</td>");
            sb.AppendLine($"          <td>{Encode(result.ScriptSystem)}</td>");
            sb.AppendLine($"          <td class='confidence {accuracyClass}'>{accuracy:F1}%</td>");
            sb.AppendLine($"          <td>{result.Confidence:F1}%</td>");
            sb.AppendLine($"          <td>{result.ProcessingTime.TotalSeconds:F1}s</td>");
            sb.AppendLine($"          <td>{result.PageCount}</td>");
            sb.AppendLine($"        </tr>");
        }
        sb.AppendLine("      </tbody>");
        sb.AppendLine("    </table>");
        sb.AppendLine("  </section>");

        // Detailed results per scenario
        foreach (var (result, expectedText, accuracy) in results)
        {
            var accuracyClass = accuracy >= 90 ? "high" : accuracy >= 70 ? "medium" : "low";
            sb.AppendLine($"  <section class='scenario'>");
            sb.AppendLine($"    <h2>{Encode(result.LanguageName)} <span class='badge {accuracyClass}'>{accuracy:F1}%</span></h2>");
            sb.AppendLine($"    <p class='meta'>Script: {Encode(result.ScriptSystem)} | Engine confidence: {result.Confidence:F1}% | Processing time: {result.ProcessingTime.TotalSeconds:F1}s | Pages: {result.PageCount}</p>");

            sb.AppendLine("    <div class='comparison'>");

            sb.AppendLine("      <div class='column'>");
            sb.AppendLine("        <h3>Extracted Text</h3>");
            sb.AppendLine($"        <pre>{Encode(result.ExtractedText.Trim())}</pre>");
            sb.AppendLine("      </div>");

            sb.AppendLine("      <div class='column'>");
            sb.AppendLine("        <h3>Expected Text</h3>");
            sb.AppendLine($"        <pre>{Encode(expectedText)}</pre>");
            sb.AppendLine("      </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("  </section>");
        }

        // Footer
        sb.AppendLine("  <footer>");
        sb.AppendLine("    <p>Generated by IronOCR Languages Guide Demo — <a href='https://ironsoftware.com/csharp/ocr/'>ironsoftware.com</a></p>");
        sb.AppendLine("  </footer>");

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        return sb.ToString();
    }

    private static string Encode(string text) =>
        System.Net.WebUtility.HtmlEncode(text);

    private static string GetReportCss() => @"
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body {
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
            background: #f5f5f5; color: #333; line-height: 1.6;
        }
        header {
            background: linear-gradient(135deg, #1a1a2e, #16213e);
            color: white; padding: 40px; text-align: center;
        }
        header h1 { font-size: 2em; margin-bottom: 8px; }
        .subtitle { opacity: 0.8; font-size: 1.1em; }
        section { max-width: 1100px; margin: 30px auto; padding: 0 20px; }
        h2 { font-size: 1.4em; margin-bottom: 16px; color: #1a1a2e; }
        .summary table {
            width: 100%; border-collapse: collapse;
            background: white; border-radius: 8px; overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }
        .summary th { background: #1a1a2e; color: white; padding: 12px 16px; text-align: left; }
        .summary td { padding: 12px 16px; border-bottom: 1px solid #eee; }
        .confidence.high { color: #2e7d32; font-weight: bold; }
        .confidence.medium { color: #f57f17; font-weight: bold; }
        .confidence.low { color: #c62828; font-weight: bold; }
        .badge {
            display: inline-block; padding: 2px 10px; border-radius: 12px;
            font-size: 0.8em; font-weight: bold; margin-left: 8px;
        }
        .badge.high { background: #e8f5e9; color: #2e7d32; }
        .badge.medium { background: #fff8e1; color: #f57f17; }
        .badge.low { background: #ffebee; color: #c62828; }
        .scenario {
            background: white; padding: 24px; border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1); margin-bottom: 24px;
        }
        .meta { color: #666; font-size: 0.9em; margin-bottom: 16px; }
        .comparison { display: flex; gap: 20px; }
        .column { flex: 1; }
        .column h3 { font-size: 1em; color: #555; margin-bottom: 8px; }
        .column pre {
            background: #f8f9fa; border: 1px solid #e0e0e0; border-radius: 4px;
            padding: 16px; white-space: pre-wrap; word-wrap: break-word;
            font-size: 0.9em; max-height: 400px; overflow-y: auto;
        }
        footer {
            text-align: center; padding: 30px; color: #999; font-size: 0.9em;
        }
        footer a { color: #1a73e8; text-decoration: none; }
        @media (max-width: 768px) { .comparison { flex-direction: column; } }
    ";
}
