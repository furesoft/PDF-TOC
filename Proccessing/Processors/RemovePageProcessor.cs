using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class RemovePageProcessor : IPdfProcessor
{
    public int Page { get; set; }
    
    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Pages.RemoveAt(Page);
    }
}