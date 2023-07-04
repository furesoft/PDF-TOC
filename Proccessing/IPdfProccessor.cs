using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public interface IPdfProccessor
{
    void Invoke(PdfDocument document, PdfProccessor processor);
}