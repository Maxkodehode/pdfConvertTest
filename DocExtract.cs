using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.ReadingOrderDetector;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace pdfTest;

class DocExtracter
{
    public void extracter()
    {
        var sourcePdfPath = "/home/eikichi/Dev/TestingFolder/AG60.pdf";
        var outputPath = "/home/eikichi/RiderProjects/pdfConvertTest/outputPath/result.pdf";
        var pageNumber = 1;
        using (var document = PdfDocument.Open(sourcePdfPath))
        {
            var builder = new PdfDocumentBuilder { };
            PdfDocumentBuilder.AddedFont font = builder.AddStandard14Font(Standard14Font.Helvetica);

            foreach (var page in document.GetPages()) // loop all pages
            {
                var pageBuilder = builder.AddPage(document, page.Number);
                pageBuilder.SetStrokeColor(0, 255, 0);

                var letters = page.Letters;

                var wordExtractor = NearestNeighbourWordExtractor.Instance;
                var words = wordExtractor.GetWords(letters);

                var pageSegmenter = DocstrumBoundingBoxes.Instance;
                var textBlocks = pageSegmenter.GetBlocks(words);

                var readingOrder = UnsupervisedReadingOrderDetector.Instance;
                var orderedTextBlocks = readingOrder.Get(textBlocks);

                foreach (var block in orderedTextBlocks)
                {
                    var bbox = block.BoundingBox;
                    pageBuilder.DrawRectangle(bbox.BottomLeft, bbox.Width, bbox.Height);
                    pageBuilder.AddText(block.ReadingOrder.ToString(), 8, bbox.TopLeft, font);
                }
            }

            byte[] fileBytes = builder.Build();
            File.WriteAllBytes(outputPath, fileBytes);
        }
    }
}
