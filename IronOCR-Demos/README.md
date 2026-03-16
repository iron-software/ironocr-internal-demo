# IronOCR Demos

Internal demonstration repository showcasing IronOCR's capabilities through practical, runnable examples designed for sales demonstrations and customer integration guidance.

## Prerequisites

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- A valid IronOCR license key (see [License Setup](#license-setup))

## Quick Start

```bash
# Clone the repository
git clone <repository-url>
cd ironocr-internal-demo

# Configure your license key (see License Setup below)

# Restore packages and run
cd IronOCR-Demos
dotnet restore
dotnet run
```

## License Setup

1. Open `IronOCR-Demos/appsettings.json`
2. Replace `YOUR-LICENSE-KEY-HERE` with your IronOCR license key:
   ```json
   {
     "IronOcr.LicenseKey": "IRONOCR-YOUR-ACTUAL-KEY-HERE"
   }
   ```
3. The demo will also work in trial mode (with watermarks) if no key is provided

For more details, see [IronOCR License Keys](https://ironsoftware.com/csharp/ocr/get-started/license-keys/).

## Available Demos

| Demo | Description | Guide |
|------|-------------|-------|
| **Languages Guide** | Multi-language PDF OCR across 4 script systems (Latin, CJK, Cyrillic) with HTML comparison report | [README](Demos/LanguagesGuide/README.md) |

## Project Structure

```
IronOCR-Demos/
├── Program.cs              # Main entry point — demo selection menu
├── appsettings.json        # License key configuration
├── Demos/
│   ├── Utils.cs            # Shared utilities (licensing, path helpers)
│   └── LanguagesGuide/     # Multi-language OCR demo
│       ├── LanguagesGuideDemo.cs        # Core OCR logic
│       ├── LanguagesGuideController.cs  # CLI + HTML report
│       ├── Examples/                    # Sample PDFs
│       ├── ExpectedText/                # Known text for accuracy comparison
│       └── README.md                    # Demo-specific guide
├── Output/                 # Generated reports (gitignored)
Tools/
└── GenerateSamplePdfs/     # Utility to create sample PDFs
```

## Generating Sample PDFs

The sample PDFs in `Demos/LanguagesGuide/Examples/` are generated programmatically:

```bash
cd Tools/GenerateSamplePdfs
dotnet run
```

This creates PDFs with known text content in English, Spanish, Chinese, and Russian, plus a multi-language document.

## Adding a New Demo

1. Create a new folder under `Demos/` (e.g., `Demos/Filters/`)
2. Add a demo class for IronOCR logic and a controller class for CLI interaction
3. Register the demo in `Program.cs` as a new menu option
4. Add a `README.md` to the demo folder with usage instructions
