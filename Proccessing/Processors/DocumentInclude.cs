using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PDF_TOC.Proccessing.Processors;

public class DocumentInclude : PdfProcessor
{
    public DocumentInclude(string filename, bool toc)
    {
        Filename = filename;
        Pages = PageRange.All();
        IncludeToC = toc;
    }

    public DocumentInclude(string filename, PageRanges range, bool toc)
    {
        Filename = filename;
        Pages = range;
        IncludeToC = toc;
    }

    public DocumentInclude(string filename, string range, bool toc)
    {
        Filename = filename;
        Pages = PageRanges.Parse(range);
        IncludeToC = toc;
    }

    public string Filename { get; set; }

    public PageRanges Pages { get; set; }

    public bool IncludeToC { get; set; }

    public override void Invoke(PdfDocument document, PdfProccessor processor)
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

            //todo: include page to toc if flag is true

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