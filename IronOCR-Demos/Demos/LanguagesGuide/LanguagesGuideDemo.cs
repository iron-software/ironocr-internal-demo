using IronOcr;

namespace IronOCR_Demos.Demos.LanguagesGuide;

/// <summary>
/// Core OCR logic for the Languages Guide demo.
/// Each method demonstrates IronOCR processing a PDF in a specific language.
/// </summary>
public class LanguagesGuideDemo
{
    /// <summary>
    /// Result from a single language OCR scenario.
    /// </summary>
    public class ScenarioResult
    {
        public string LanguageName { get; set; } = "";
        public string ScriptSystem { get; set; } = "";
        public string ExtractedText { get; set; } = "";
        public double Confidence { get; set; }
        public int PageCount { get; set; }
        public TimeSpan ProcessingTime { get; set; }
    }

    // ── English ────────────────────────────────────────────────────────────
    /// <summary>
    /// Baseline OCR using the default English language.
    /// </summary>
    public ScenarioResult ReadEnglish(string pdfPath)
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.English;

        return RunOcr(ocr, pdfPath, "English", "Latin");
    }

    // ── Spanish ────────────────────────────────────────────────────────────
    /// <summary>
    /// OCR using the Spanish language pack for Latin-script text.
    /// </summary>
    public ScenarioResult ReadSpanish(string pdfPath)
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.Spanish;

        return RunOcr(ocr, pdfPath, "Spanish", "Latin");
    }

    // ── Chinese Simplified ─────────────────────────────────────────────────
    /// <summary>
    /// OCR using the Chinese Simplified language pack for CJK characters.
    /// </summary>
    public ScenarioResult ReadChinese(string pdfPath)
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.ChineseSimplified;

        return RunOcr(ocr, pdfPath, "Chinese Simplified", "CJK");
    }

    // ── Russian ────────────────────────────────────────────────────────────
    /// <summary>
    /// OCR using the Russian language pack for Cyrillic script.
    /// </summary>
    public ScenarioResult ReadRussian(string pdfPath)
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.Russian;

        return RunOcr(ocr, pdfPath, "Russian", "Cyrillic");
    }

    // ── Multi-Language ─────────────────────────────────────────────────────
    /// <summary>
    /// Flagship scenario: processes a document containing text in multiple languages
    /// using AddSecondaryLanguage() to enable simultaneous recognition.
    /// </summary>
    public ScenarioResult ReadMultiLanguage(string pdfPath)
    {
        var ocr = new IronTesseract();

        // Primary language
        ocr.Language = OcrLanguage.English;

        // Add secondary languages for mixed-script documents
        ocr.AddSecondaryLanguage(OcrLanguage.Spanish);
        ocr.AddSecondaryLanguage(OcrLanguage.ChineseSimplified);
        ocr.AddSecondaryLanguage(OcrLanguage.Russian);

        return RunOcr(ocr, pdfPath, "Multi-Language", "Mixed (Latin, CJK, Cyrillic)");
    }

    // ── Shared OCR Runner ──────────────────────────────────────────────────
    private ScenarioResult RunOcr(IronTesseract ocr, string pdfPath, string languageName, string scriptSystem)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        using var input = new OcrInput();
        input.LoadPdf(pdfPath);

        var result = ocr.Read(input);
        stopwatch.Stop();

        return new ScenarioResult
        {
            LanguageName = languageName,
            ScriptSystem = scriptSystem,
            ExtractedText = result.Text,
            Confidence = result.Confidence,
            PageCount = result.Pages.Length,
            ProcessingTime = stopwatch.Elapsed
        };
    }
}
