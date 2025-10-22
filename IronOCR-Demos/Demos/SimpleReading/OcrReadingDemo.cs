using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using IronOcr;
using IronSoftware;
using IronSoftware.Drawing;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static IronOcr.OcrResult;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace IronOCR_Demos.Demos.SimpleReading
{
    internal class OcrReadingDemo
    {

#region Input
        public OcrResult ReadImageFromDrawingImage(string imagePath, Rectangle scanRegion)
        {
            // Instantiate the OCR engine
            var ocr = new IronTesseract();
            ocr.Configuration.RenderSearchablePdf = true;

            // Add image
            using var imageInput = new OcrImageInput(imagePath);
            // Perform OCR
            OcrResult result = ocr.Read(imageInput);

            // Return the extracted text
            return result;
        }


        public OcrResult ReadImageFromDrawingImageWithRegionSelection(string imagePath, Rectangle? scanRegion = null)
        {
            // Instantiate IronTesseract
            IronTesseract ocr = new IronTesseract();
            ocr.Configuration.RenderSearchablePdf = true;

            // Add image
            OcrImageInput imageInput;
            if (scanRegion != null)
            {
                imageInput = new OcrImageInput(imagePath, ContentArea: scanRegion);
                imageInput.SaveAsImages($"{imagePath.Substring(imagePath.Length - 3)}_ROI.png");
            }
            else
            {
                imageInput = new OcrImageInput(imagePath);
            }
            // Perform OCR
            OcrResult ocrResult = ocr.Read(imageInput);

            return ocrResult;
        }


        public OcrResult ReadPdf(PdfDocument pdfPath, Rectangle[] scanRegion = null, bool savePDFROI = true)
        {
            // Instantiate the OCR engine
            var ocr = new IronTesseract();
            ocr.Configuration.RenderSearchablePdf = true;
            // Create PDF input with or without scan region
            var ocrInput = new OcrInput();
            //ocrInput.LoadPdf(Document pdfPath, ContentArea: scanRegion);

            if (savePDFROI)
            {
                ocrInput.SaveAsImages($"{pdfPath}_ROI.png");
            }

            // Perform OCR
            OcrResult ocrResult = ocr.Read(ocrInput);

            // Return the extracted text
            return ocrResult;
        }
        #endregion Input

        public void OutputResult(OcrResult ocrResult, OutputTypes typeSlection, string filePath, string fileName)
        {
            switch (typeSlection)
            {
                case OutputTypes.JSON:
                    ocrResult.SaveJsonAs($"{filePath}/{fileName}.json");
                    break;
                case OutputTypes.SearchablePDF:
                    ocrResult.SaveAsSearchablePdf($"{filePath}/{fileName}.pdf");
                    break;
                case OutputTypes.Text:
                    Console.WriteLine(ocrResult.Text.ToString());
                    break;
                case OutputTypes.TextSample:
                    var paragraphs = ocrResult.Paragraphs;
                    Console.WriteLine($"Text: {paragraphs[0].Text}");
                    Console.WriteLine($"X: {paragraphs[0].X}");
                    Console.WriteLine($"Y: {paragraphs[0].Y}");
                    Console.WriteLine($"Width: {paragraphs[0].Width}");
                    Console.WriteLine($"Height: {paragraphs[0].Height}");
                    Console.WriteLine($"Text direction: {paragraphs[0].TextDirection}");
                    break;

                case OutputTypes.HighligtParagraphs:
                    throw new NotImplementedException("HighlightParagraphs output type is not yet implemented.");
                case OutputTypes.TextFile:
                    ocrResult.SaveAsTextFile($"{filePath}/{fileName}.txt");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeSlection), typeSlection, "Unknown output type.");
            }
            
        }


        public enum OutputTypes
        {
            JSON,
            SearchablePDF,
            Text,
            TextSample,
            HighligtParagraphs,
            TextFile
        }



    }
}
