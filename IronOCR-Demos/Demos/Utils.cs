using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using IronPdf;
using IronSoftware.Drawing;

namespace IronOCR_Demos.Demos
{
    internal class Utils
    {
        public static string GetLicenseFromConfig()
        {
            // Get the project root directory (3 levels up from bin\Debug\net9.0)
            string projectRoot = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..");
            string configPath = Path.GetFullPath(projectRoot);

            var builder = new ConfigurationBuilder()
                .SetBasePath(configPath) // Sets the base path to the project root
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); // Looks for 'appsettings.json' in project root

            var configuration = builder.Build();
            return configuration["IronPDF:Licence"];
        }

        public static string GetLicence()
        {
            Console.WriteLine("Getting licence");
            string licenseKey = GetLicenseFromConfig();
            // REPLACE WITH OWN LICENCE KEY
            return licenseKey;
        }

        /// <summary>
        /// Resolves a path relative to the project root (3 levels up from bin output).
        /// Used by demos to locate their data files.
        /// </summary>
        public static string GetProjectRootPath(params string[] pathSegments)
        {
            string projectRoot = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..");
            string basePath = Path.GetFullPath(projectRoot);
            return Path.Combine(new[] { basePath }.Concat(pathSegments).ToArray());
        }

        /// <summary>
        /// Adds a semi-transparent red rectangle to a specified page of a PdfDocument object.
        /// </summary>
        /// <param name="pdfDocument">The PdfDocument object to modify.</param>
        /// <param name="pageNumber">The 1-based index of the page where the rectangle will be drawn.</param>
        /// <param name="x">The X-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="y">The Y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public void AddRegionOfInterestToPDF(PdfDocument pdfDocument, int pageNumber, float x, float y, float width, float height)
        {
            // Validate the page number. Pages are 0-indexed in the library.
            int pageIndex = pageNumber - 1;
            if (pageIndex < 0 || pageIndex >= pdfDocument.Pages.Count)
            {
                Console.WriteLine($"Error: Page number {pageNumber} is out of bounds. The PDF has {pdfDocument.Pages.Count} pages.");
                return;
            }

            // Define the region of interest using the input coordinates and dimensions.
            var regionOfInterest = new RectangleF(x, y, width, height);

            // Define the semi-transparent color for the highlight.
            var highlightColor = new IronSoftware.Drawing.Color(255, 0, 0, 128); // Semi-transparent red
            double lineWidth = 5;
            var fillColor = new IronSoftware.Drawing.Color("#000000");

            // Draw the rectangle on the specified page.
            pdfDocument.DrawRectangle(
                pageIndex,
                regionOfInterest,
                highlightColor,
                fillColor,
                lineWidth
            );

            Console.WriteLine($"Successfully added a region of interest to page {pageNumber}.");
        }
    }
}
