using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public class RemovePageProccessor : IPdfProccessor
{
    public int Page { get; set; }
    
    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Pages.RemoveAt(Page);
    }
}