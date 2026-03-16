# Languages Guide — Multi-Language PDF OCR

This demo showcases IronOCR's ability to accurately extract text from PDFs written in different languages and script systems. It demonstrates a key competitive advantage: multi-language OCR with minimal configuration.

## What It Demonstrates

- OCR across **4 different script systems**: Latin, CJK, and Cyrillic
- **Language pack installation** via NuGet — no manual configuration
- **Multi-language document processing** using `AddSecondaryLanguage()`
- **Confidence scoring** to measure extraction accuracy
- **Side-by-side comparison** of extracted vs expected text via HTML report

## Scenarios

| # | Scenario | Language Pack | Script | Description |
|---|----------|---------------|--------|-------------|
| 1 | English | `OcrLanguage.English` | Latin | Baseline — business letter |
| 2 | Spanish | `OcrLanguage.Spanish` | Latin | Accented characters — product description |
| 3 | Chinese Simplified | `OcrLanguage.ChineseSimplified` | CJK | Chinese characters — business document |
| 4 | Russian | `OcrLanguage.Russian` | Cyrillic | Cyrillic script — technical specification |
| 5 | Multi-Language | Multiple via `AddSecondaryLanguage()` | Mixed | **Flagship** — document with all 4 languages |

## Running the Demo

1. From the main menu, select **Languages Guide**
2. Choose a specific scenario (1-5) or **Run all scenarios** (6)
3. An HTML report will be generated in the `Output/` directory and opened in your browser

## Understanding the Report

The HTML report shows:

- **Summary table** — all scenarios with confidence scores at a glance
- **Per-scenario details** — extracted text vs expected text side-by-side
- **Confidence indicators**:
  - Green (90%+) — excellent accuracy
  - Yellow (70-90%) — acceptable accuracy
  - Red (<70%) — review needed

## Customization

### Adding Your Own PDFs

Place your PDF files in the `Examples/` directory and add corresponding expected text files in `ExpectedText/`. Then add a new entry in `LanguagesGuideController._scenarios` and a corresponding method in `LanguagesGuideDemo`.

### Adding More Languages

1. Install the language pack NuGet package:
   ```
   dotnet add package IronOcr.Languages.<LanguageName>
   ```
2. Add a new method in `LanguagesGuideDemo.cs`:
   ```csharp
   public ScenarioResult ReadJapanese(string pdfPath)
   {
       var ocr = new IronTesseract();
       ocr.Language = OcrLanguage.Japanese;
       return RunOcr(ocr, pdfPath, "Japanese", "CJK");
   }
   ```
3. Register the scenario in `LanguagesGuideController._scenarios`

## Language Pack Reference

IronOCR supports 125+ languages. Common language packs:

| Language | NuGet Package | OcrLanguage Enum |
|----------|---------------|------------------|
| English | (included) | `OcrLanguage.English` |
| Spanish | `IronOcr.Languages.Spanish` | `OcrLanguage.Spanish` |
| Chinese (Simplified) | `IronOcr.Languages.ChineseSimplified` | `OcrLanguage.ChineseSimplified` |
| Chinese (Traditional) | `IronOcr.Languages.ChineseTraditional` | `OcrLanguage.ChineseTraditional` |
| Russian | `IronOcr.Languages.Russian` | `OcrLanguage.Russian` |
| Japanese | `IronOcr.Languages.Japanese` | `OcrLanguage.Japanese` |
| Arabic | `IronOcr.Languages.Arabic` | `OcrLanguage.Arabic` |
| French | `IronOcr.Languages.French` | `OcrLanguage.French` |
| German | `IronOcr.Languages.German` | `OcrLanguage.German` |
| Korean | `IronOcr.Languages.Korean` | `OcrLanguage.Korean` |
| Hindi | `IronOcr.Languages.Hindi` | `OcrLanguage.Hindi` |

For the full list, see the [IronOCR Languages documentation](https://ironsoftware.com/csharp/ocr/languages/).
