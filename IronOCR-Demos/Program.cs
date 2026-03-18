using IronOCR_Demos.Controllers;
using IronOCR_Demos.Demos;
using IronOCR_Demos.Demos.LanguagesGuide;
using System;
using System.IO;

namespace IronOCR_Demos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IronOcr.License.LicenseKey = Utils.GetLicence();
            IronPdf.License.LicenseKey = Utils.GetLicence();

            //Logging enables
            IronPdf.Logging.Logger.LogFilePath = "Default.log";
            IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;

            try
            {
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("╔══════════════════════════════════════╗");
                    Console.WriteLine("║         IronOCR Demos                ║");
                    Console.WriteLine("╚══════════════════════════════════════╝");
                    Console.WriteLine();
                    Console.WriteLine("  1. Simple Reading - OCR File Processor");
                    Console.WriteLine("  2. Languages Guide - Multi-language PDF OCR");
                    Console.WriteLine("  0. Exit");
                    Console.WriteLine();
                    Console.Write("Select a demo [0-2]: ");

                    var input = Console.ReadLine()?.Trim();

                    switch (input)
                    {
                        case "1":
                            RunSimpleReading();
                            break;
                        case "2":
                            var languagesController = new LanguagesGuideController();
                            languagesController.Run();
                            break;
                        case "0":
                            Console.WriteLine("\nThank you for using IronOCR Demos!");
                            return;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Launches the Simple Reading demo (original OCR File Processor).
        /// </summary>
        private static void RunSimpleReading()
        {
            // Get the project root directory (3 levels up from bin\Debug\net9.0)
            string projectRoot = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..");
            string inputDirectory = Path.Combine(projectRoot, @"Demos\SimpleReading\Examples");

            // Normalize the path to remove the ".." segments
            inputDirectory = Path.GetFullPath(inputDirectory);

            Console.WriteLine("Starting OCR File Processor...");
            Console.WriteLine($"Looking for files in: {inputDirectory}");
            Console.WriteLine();

            // Create and run the file selector controller
            var fileSelectorController = new FileSelectorCliController(inputDirectory);
            fileSelectorController.Run();
        }
    }
}
