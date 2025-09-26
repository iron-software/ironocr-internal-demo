using IronOCR_Demos.Controllers;
using IronOCR_Demos.Demos;
using System;
using System.IO;

namespace IronOCR_Demos
{
    internal class Program
    {
        static void Main(string[] args)
        {

            License.LicenseKey = Utils.GetLicence();

            try
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

                Console.WriteLine("\nThank you for using OCR File Processor!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}