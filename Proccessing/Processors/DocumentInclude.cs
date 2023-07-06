using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PDF_TOC.Proccessing.Processors;

public class DocumentInclude : IPdfProcessor
{
    public DocumentInclude(string filename)
    {
        Filename = filename;
        Pages = PageRange.All();
    }

    public DocumentInclude(string filename, PageRanges range)
    {
        Filename = filename;
        Pages = range;
    }

    public DocumentInclude(string filename, string range)
    {
        Filename = filename;
        Pages = PageRanges.Parse(range);
    }

    public string Filename { get; set; }
    public PageRanges Pages { get; set; }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var doc = PdfReader.Open(Filename, PdfDocumentOpenMode.Import);

        foreach (var range in Pages)
        {
            if (range.Start == 0 && range.End == 0)
            {
                foreach (var page in doc.Pages)
                {
                    document.Pages.Add(page);
                }

                return;
            }


            if (range.Start == range.End)
            {
                document.Pages.Add(doc.Pages[range.Start]);
            }

            foreach (var pageIndex in range.GetIndices())
            {
                var importedPage = doc.Pages[pageIndex];

                document.Pages.Add(importedPage);
            }
        }
    }
}