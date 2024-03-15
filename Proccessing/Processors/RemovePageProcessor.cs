using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class RemovePageProcessor : PdfProcessor
{
    public int Page { get; set; }
    
    public override void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Pages.RemoveAt(Page);
    }
}