using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PDF_TOC.Proccessing;

public class DocumentInclude : IPdfProccessor
{


    public DocumentInclude(string filename)
    {
        Filename = filename;
        Pages = PageRange.All();
    }
    
    public DocumentInclude(string filename, PageRange range)
        {
            Filename = filename;
            Pages = range;
        }
        
    public DocumentInclude(string filename, string range)
    {
        Filename = filename;
        Pages = PageRange.Parse(range);
    }

    public string Filename { get; set; }
    public PageRange Pages { get; set; }
    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var doc = PdfReader.Open(Filename, PdfDocumentOpenMode.Import);

        if (Pages.Start == 0 && Pages.End == 0)
        {
            foreach (var page in doc.Pages)
            {
                document.Pages.Add(page);
            }
            return;
        }
        
        if (Pages.Start == Pages.End)
        {
            document.Pages.Add(doc.Pages[Pages.Start]);
        }
        
        foreach (var pageIndex in Pages.GetIndices())
        {
            var importedPage = doc.Pages[pageIndex];

            document.Pages.Add(importedPage);
        }
    }
}