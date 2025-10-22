# Simple Reading Demo

A comprehensive OCR demonstration that showcases IronOCR's text extraction capabilities through an interactive command-line interface.

## Overview

This example demonstrates how to extract text from images and PDF documents using IronOCR. It features a user-friendly CLI that allows you to select files, choose output formats, and process documents with minimal setup.

## Features

- **Multi-format Input Support**: Process images (PNG, JPG, BMP, TIFF, GIF) and PDF files
- **Interactive File Selection**: Browse and select files from a numbered menu
- **Multiple Output Options**: Export results in various formats
- **Searchable PDF Generation**: Create PDFs with embedded OCR text layer
- **Detailed Text Analysis**: Extract paragraph-level details including coordinates

## Getting Started

### Adding Input Files

1. Navigate to the `Examples` directory:
   ```
   IronOCR_Demos/Demos/SimpleReading/Examples/
   ```

2. Add your test files:
   - Copy image files (.png, .jpg, .jpeg, .bmp, .tiff, .tif, .gif)
   - Copy PDF files (.pdf)
   - Multiple files can be added at once

3. Run the application:
   ```bash
   dotnet run
   ```

### Using the Application

1. **Select a File**: Choose from the numbered list of available files
2. **Choose Output Format**: Select how you want to view or save the results
3. **Specify Output Location** (if applicable): Choose where to save output files
4. **View Results**: Review the extracted text or generated files

## Core Methods

The `OcrReadingDemo` class provides the following methods:

### `ReadImageFromDrawingImage(string imagePath, Rectangle scanRegion)`

Reads text from an image file using IronOCR.

**Parameters:**
- `imagePath` (string): Full path to the image file
- `scanRegion` (Rectangle): Specific area of the image to scan

**Returns:** `OcrResult` containing extracted text and metadata

**Use Case:** Basic OCR on complete images with optional region selection

---

### `ReadImageFromDrawingImageWithRegionSelection(string imagePath, Rectangle? scanRegion = null)`

Advanced image reading with optional region-of-interest functionality.

**Parameters:**
- `imagePath` (string): Full path to the image file
- `scanRegion` (Rectangle?, optional): Specific area to scan; if null, processes entire image

**Returns:** `OcrResult` containing extracted text and metadata

**Features:**
- Automatically saves ROI preview when region is specified
- Generates searchable PDF output
- Flexible region selection

**Use Case:** When you need to focus on specific areas of an image (e.g., reading only a header or specific field)

---

### `ReadPdf(PdfDocument pdfPath, Rectangle[] scanRegion = null, bool savePDFROI = true)`

Extracts text from PDF documents with optional region targeting.

**Parameters:**
- `pdfPath` (PdfDocument): PDF document object to process
- `scanRegion` (Rectangle[], optional): Array of regions to scan across pages
- `savePDFROI` (bool): Whether to save region-of-interest preview images

**Returns:** `OcrResult` containing extracted text and metadata

**Features:**
- Multi-page PDF support
- Optional ROI visualization
- Searchable PDF generation

**Use Case:** Processing PDF documents, especially scanned documents or images embedded in PDFs

---

### `OutputResult(OcrResult ocrResult, OutputTypes typeSelection, string filePath, string fileName)`

Handles the output of OCR results in various formats.

**Parameters:**
- `ocrResult` (OcrResult): The OCR result to output
- `typeSelection` (OutputTypes): Desired output format
- `filePath` (string): Directory path for file outputs
- `fileName` (string): Name for output files (without extension)

**Supported Output Types:**

#### `JSON`
Exports detailed OCR results in JSON format, including:
- Full text content
- Paragraph-level data
- Word coordinates
- Confidence scores

**Output:** `{fileName}.json`

#### `SearchablePDF`
Creates a PDF with an invisible text layer overlay, making scanned documents searchable.

**Output:** `{fileName}.pdf`

#### `Text`
Displays extracted text directly in the console.

**Output:** Console output only

#### `TextSample`
Shows detailed information about the first paragraph, including:
- Text content
- X, Y coordinates
- Width and height
- Text direction

**Output:** Console output with structured data

#### `TextFile`
Saves extracted text as a plain text file.

**Output:** `{fileName}.txt`

#### `HighlightParagraphs`
*Currently not implemented* - Future feature to visually highlight detected paragraphs

## Output Types Enum

```csharp
public enum OutputTypes
{
    JSON,                  // Structured JSON export
    SearchablePDF,         // PDF with OCR text layer
    Text,                  // Console text output
    TextSample,            // First paragraph details
    HighligtParagraphs,    // Visual highlighting (not implemented)
    TextFile               // Plain text file
}
```

## Example Workflows

### Workflow 1: Basic Image OCR

1. Add an image to `Examples/` folder
2. Run the application
3. Select your image from the list
4. Choose "Text" for console output
5. View extracted text immediately

### Workflow 2: Creating Searchable PDFs

1. Add a scanned document or image to `Examples/`
2. Run the application
3. Select your file
4. Choose "Searchable PDF"
5. Specify output location
6. Open the generated PDF - text is now searchable

### Workflow 3: Detailed Text Analysis

1. Add a document with structured content
2. Run the application
3. Select your file
4. Choose "JSON" output
5. Open the JSON file to analyze:
   - Individual paragraph positions
   - Word-level coordinates
   - Confidence scores
   - Text hierarchy

### Workflow 4: Region-Specific Reading

To read only specific areas (modify code):

```csharp
// Define region coordinates
Rectangle region = new Rectangle(100, 100, 400, 200);

// Read only that region
var result = ocrDemo.ReadImageFromDrawingImageWithRegionSelection(
    "path/to/image.png", 
    region
);
```

## Configuration

### OCR Engine Configuration

The demo uses default IronTesseract configuration with searchable PDF rendering enabled:

```csharp
var ocr = new IronTesseract();
ocr.Configuration.RenderSearchablePdf = true;
```


## Technical Details

### Dependencies
- `IronOcr` - OCR engine
- `IronSoftware.Drawing` - Image handling

### Supported File Formats

**Input:**
- Images: PNG, JPG, JPEG, BMP, TIFF, TIF, GIF
- Documents: PDF

**Output:**
- JSON (structured data)
- PDF (searchable)
- TXT (plain text)

### System Requirements
- .NET Core 9.0 or later
- Minimum 4GB RAM recommended
- Windows, macOS, or Linux

## Future Enhancements

Planned features for upcoming versions:
- Paragraph highlighting visualization
- Multi-language support demonstration

## Related Documentation

- [IronOCR Documentation](https://ironsoftware.com/csharp/ocr/docs/)
- [Main Repository README](../../README.md)
- [API Reference](https://ironsoftware.com/csharp/ocr/object-reference/)

---

**Example Type**: OCR Reading  
**Complexity**: Beginner to Intermediate  
**Estimated Setup Time**: 5 minutes  
**Last Updated**: October 2025