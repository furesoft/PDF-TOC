using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class MetadataProcessor : PdfProcessor
{
    private string author;
    private string title;

    public MetadataProcessor(string author, string title)
    {
        this.author = author;
        this.title = title;
    }

    public override void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Info.Author = author;
        document.Info.Title = title;
    }
}