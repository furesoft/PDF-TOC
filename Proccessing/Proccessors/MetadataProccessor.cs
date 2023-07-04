using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public class MetadataProccessor : IPdfProccessor
{
    private string author;
    private string title;

    public MetadataProccessor(string author, string title)
    {
        this.author = author;
        this.title = title;
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Info.Author = author;
        document.Info.Title = title;
    }
}