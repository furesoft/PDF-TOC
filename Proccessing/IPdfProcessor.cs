using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public interface IPdfProcessor
{
    void Invoke(PdfDocument document, PdfProccessor processor);
}