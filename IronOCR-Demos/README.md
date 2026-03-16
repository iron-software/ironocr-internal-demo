# IronOCR Internal Demo Repository

Welcome to the IronOCR demonstration repository. This collection showcases IronOCR's capabilities through practical, runnable examples designed for sales demonstrations and customer integration guidance.

## Overview

This repository contains demonstration projects that highlight IronOCR's key features and best practices. Each example is designed to be clear, maintainable, and easily adaptable for customer-specific use cases.

## Repository Structure

```
IronOCR-Demos/
├── Demos/
│   ├── SimpleReading/          # OCR reading demonstrations
│   │   ├── Examples/           # Input files directory
│   │   ├── OcrReadingDemo.cs   # Core OCR functionality
│   │   └── README.md           # Detailed example documentation
│   └── LanguagesGuide/         # Multi-language OCR demonstrations
│       ├── Examples/           # Sample PDFs per language
│       ├── ExpectedText/       # Known text for accuracy comparison
│       ├── LanguagesGuideDemo.cs       # Core OCR logic
│       ├── LanguagesGuideController.cs # CLI + HTML report
│       └── README.md           # Detailed guide documentation
├── Controllers/
│   └── FileSelectorCliController.cs  # CLI interface
└── Program.cs                  # Application entry point (demo selection menu)
```

## Getting Started

### Prerequisites

- .NET Core SDK (version 9.0 or later)
- Valid IronOCR license key
- Visual Studio 2022 or VS Code (recommended)

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd IronOCR-Demos
   ```

2. **Configure your license**
   - Add your IronOCR license key to the project
   - The license is loaded via `Utils.GetLicence()` in `Program.cs`

3. **Add input files**
   - Navigate to `Demos/SimpleReading/Examples/` for Simple Reading
   - For Languages Guide, generate sample PDFs (see below)

4. **Build and run**
   ```bash
   dotnet build
   dotnet run
   ```

## Available Demonstrations

### Simple Reading Example

A comprehensive OCR file processor with an interactive CLI interface. This example demonstrates:

- Reading text from images and PDFs
- Multiple output format options
- Region-of-interest (ROI) scanning
- Searchable PDF generation

**[View detailed documentation →](Demos/SimpleReading/README.md)**

### Languages Guide

Multi-language PDF OCR across 4 script systems (Latin, CJK, Cyrillic). Generates an HTML report comparing extracted text against known expected text with confidence scores. Demonstrates:

- OCR in English, Spanish, Chinese Simplified, and Russian
- Language pack installation and configuration via NuGet
- Multi-language document processing using `AddSecondaryLanguage()`
- Side-by-side accuracy comparison with confidence scoring

**[View detailed documentation →](Demos/LanguagesGuide/README.md)**

## Generating Sample PDFs (Languages Guide)

The sample PDFs for the Languages Guide are generated programmatically:

```bash
cd Tools/GenerateSamplePdfs
dotnet run
```

This creates PDFs with known text content in English, Spanish, Chinese, and Russian, plus a multi-language document.

## Key Features Demonstrated

### Readability
- Clear method naming and structure
- Comprehensive commenting
- Logical code organization
- Easy-to-follow examples

### Performance
- Efficient file processing
- Optimized OCR configuration
- Resource management best practices

### Scalability
- Modular design for easy extension
- Reusable components
- Flexible input handling

### Cross-Platform
- .NET Core compatibility
- Platform-agnostic file operations
- Consistent behavior across operating systems

## Output Formats

The demonstrations support multiple output formats:

- **Console Text**: Direct text output to the console
- **Text File**: Plain text file (.txt)
- **JSON**: Structured data with paragraph details (.json)
- **Searchable PDF**: PDF with embedded OCR text layer (.pdf)
- **Text Sample**: Detailed paragraph analysis with coordinates
- **HTML Report**: Side-by-side comparison with confidence scores (Languages Guide)

## Adding Your Own Files

1. Navigate to the `Examples` folder for the demo you want to run
2. Copy your image or PDF files into this directory
3. Run the application and select your file from the menu
4. Choose your desired output format

## Adding a New Demo

1. Create a new folder under `Demos/` (e.g., `Demos/Filters/`)
2. Add a demo class for IronOCR logic and a controller class for CLI interaction
3. Register the demo in `Program.cs` as a new menu option
4. Add a `README.md` to the demo folder with usage instructions

## Contributing

This is an internal demonstration repository. For suggestions or improvements, please contact the development team.


**Version**: 1.0
**Last Updated**: October 2025
**Maintained By**: IronSoftware Sales Engineer Team
