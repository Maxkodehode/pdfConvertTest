using System.Collections.Generic;
using pdfTest;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace pdfTest;

class Program
{
    static void Main()
    {
        var extract = new DocExtracter();
        extract.extracter();
    }
}
