using magic.node;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public abstract class PdfProcessor
{
    public Node Node { get; set; }

    public abstract void Invoke(PdfDocument document, PdfProccessor processor);
}