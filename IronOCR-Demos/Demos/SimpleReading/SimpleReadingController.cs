using IronOcr;
using IronSoftware.Drawing;
using IronOCR_Demos.Demos.SimpleReading;
using System;
using System.IO;
using System.Linq;

namespace IronOCR_Demos.Controllers
{
    public class FileSelectorCliController
    {
        private readonly PDFReadingDemo _pdfReadingDemo;
        private readonly string[] _supportedImageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".tiff", ".tif", ".gif" };
        private readonly string[] _supportedPdfExtensions = { ".pdf" };
        private readonly string _inputDirectory;

        public FileSelectorCliController(string inputDirectory = "InputFiles")
        {
            _pdfReadingDemo = new PDFReadingDemo();
            _inputDirectory = Path.IsPathRooted(inputDirectory)
                ? inputDirectory
                : Path.Combine(Directory.GetCurrentDirectory(), inputDirectory);
        }

        public void Run()
        {
            // Check if input directory exists
            if (!Directory.Exists(_inputDirectory))
            {
                Console.WriteLine($"Input directory '{_inputDirectory}' does not exist.");
                Console.WriteLine("Please create the directory and add files to process.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== OCR File Processor ===");
                Console.WriteLine($"Input Directory: {_inputDirectory}");
                Console.WriteLine();

                // Select file
                string selectedFile = SelectFile();
                if (string.IsNullOrEmpty(selectedFile))
                {
                    if (ConfirmExit())
                        break;
                    continue;
                }

                // Process file
                ProcessSelectedFile(selectedFile);

                // Ask if user wants to continue
                Console.WriteLine();
                Console.Write("Process another file? (y/N): ");
                string continueChoice = Console.ReadLine()?.Trim().ToLower();
                if (continueChoice != "y" && continueChoice != "yes")
                    break;
            }
        }

        private string SelectFile()
        {
            // Get all supported files
            var allFiles = Directory.GetFiles(_inputDirectory)
                .Where(f => IsSupportedFile(f))
                .OrderBy(f => Path.GetFileName(f))
                .ToArray();

            if (allFiles.Length == 0)
            {
                Console.WriteLine($"No supported files found in '{_inputDirectory}'");
                Console.WriteLine($"Supported formats: {string.Join(", ", _supportedImageExtensions.Concat(_supportedPdfExtensions))}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }

            Console.Clear();
            Console.WriteLine($"Files in '{_inputDirectory}':");
            Console.WriteLine("=".PadRight(50, '='));

            // Display files with numbers
            for (int i = 0; i < allFiles.Length; i++)
            {
                var fileInfo = new FileInfo(allFiles[i]);
                string fileType = IsPdfFile(allFiles[i]) ? "[PDF]" : "[IMAGE]";
                Console.WriteLine($"{i + 1,3}. {fileType} {Path.GetFileName(allFiles[i])} ({FormatFileSize(fileInfo.Length)})");
            }

            Console.WriteLine();
            Console.WriteLine($"0. Exit");
            Console.WriteLine();
            Console.Write($"Select file (0-{allFiles.Length}): ");

            string input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out int selection))
            {
                Console.WriteLine("Invalid selection.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }

            if (selection == 0)
                return null;

            if (selection < 1 || selection > allFiles.Length)
            {
                Console.WriteLine("Selection out of range.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return null;
            }

            return allFiles[selection - 1];
        }

        private void ProcessSelectedFile(string filePath)
        {
            try
            {
                Console.Clear();
                Console.WriteLine($"Processing: {Path.GetFileName(filePath)}");
                Console.WriteLine("=".PadRight(50, '='));
                Console.WriteLine();

                // Show processing options
                var outputType = SelectOutputType();
                if (!outputType.HasValue)
                    return;

                Console.WriteLine();
                Console.WriteLine("Processing file... Please wait.");

                OcrResult result;

                // Process based on file type
                if (IsPdfFile(filePath))
                {
                    var pdfDocument = PdfDocument.FromFile(filePath);
                    result = _pdfReadingDemo.ReadPdf(pdfDocument, null, false);
                }
                else
                {
                    result = _pdfReadingDemo.ReadImageFromDrawingImageWithRegionSelection(filePath, null);
                }

                Console.WriteLine("OCR processing completed!");
                Console.WriteLine();

                // Get output settings if needed
                string outputPath = Directory.GetCurrentDirectory();
                string outputFileName = Path.GetFileNameWithoutExtension(filePath);

                if (outputType.Value == PDFReadingDemo.OutputTypes.JSON ||
                    outputType.Value == PDFReadingDemo.OutputTypes.SearchablePDF ||
                    outputType.Value == PDFReadingDemo.OutputTypes.TextFile)
                {
                    Console.Write($"Save to current directory? (Y/n): ");
                    string saveChoice = Console.ReadLine()?.Trim().ToLower();

                    if (saveChoice == "n" || saveChoice == "no")
                    {
                        Console.Write("Enter output directory: ");
                        string customOutput = Console.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(customOutput) && Directory.Exists(customOutput))
                        {
                            outputPath = customOutput;
                        }
                    }

                    Console.Write($"Output filename (without extension) [{outputFileName}]: ");
                    string customName = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(customName))
                    {
                        outputFileName = customName;
                    }
                }

                // Display results
                Console.WriteLine();
                Console.WriteLine("Results:");
                Console.WriteLine("-".PadRight(30, '-'));

                _pdfReadingDemo.OutputResult(result, outputType.Value, outputPath, outputFileName);

                if (outputType.Value == PDFReadingDemo.OutputTypes.JSON ||
                    outputType.Value == PDFReadingDemo.OutputTypes.SearchablePDF ||
                    outputType.Value == PDFReadingDemo.OutputTypes.TextFile)
                {
                    Console.WriteLine();

                    // Determine the file extension based on output type
                    string extension = outputType.Value switch
                    {
                        PDFReadingDemo.OutputTypes.JSON => ".json",
                        PDFReadingDemo.OutputTypes.SearchablePDF => ".pdf",
                        PDFReadingDemo.OutputTypes.TextFile => ".txt",
                        _ => ""
                    };

                    Console.WriteLine($"Output saved to: {Path.Combine(outputPath, outputFileName + extension)}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine($"Feature not implemented: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private PDFReadingDemo.OutputTypes? SelectOutputType()
        {
            Console.WriteLine("Select output format:");
            Console.WriteLine("1. Text (console output)");
            Console.WriteLine("2. Text Sample (first paragraph details)");
            Console.WriteLine("3. JSON file");
            Console.WriteLine("4. Searchable PDF");
            Console.WriteLine("5. Text File");
            Console.WriteLine("6. Highlight Paragraphs (not implemented)");
            Console.WriteLine("0. Cancel");
            Console.Write("Choose option (0-6): ");

            string choice = Console.ReadLine()?.Trim();

            return choice switch
            {
                "1" => PDFReadingDemo.OutputTypes.Text,
                "2" => PDFReadingDemo.OutputTypes.TextSample,
                "3" => PDFReadingDemo.OutputTypes.JSON,
                "4" => PDFReadingDemo.OutputTypes.SearchablePDF,
                "5" => PDFReadingDemo.OutputTypes.TextFile,
                "6" => PDFReadingDemo.OutputTypes.HighligtParagraphs,
                "0" => null,
                _ => null
            };
        }

        private bool IsSupportedFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return _supportedImageExtensions.Contains(extension) || _supportedPdfExtensions.Contains(extension);
        }

        private bool IsPdfFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return _supportedPdfExtensions.Contains(extension);
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private bool ConfirmExit()
        {
            Console.Write("Are you sure you want to exit? (y/N): ");
            string choice = Console.ReadLine()?.Trim().ToLower();
            return choice == "y" || choice == "yes";
        }
    }
}