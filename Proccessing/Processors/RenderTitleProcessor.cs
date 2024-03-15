using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class RenderTitleProcessor : PdfProcessor
{
    public XFont Font { get; set; } = new("Arial", 32);
    public PageRanges Pages { get; set; }
    public XPoint Position { get; set; }

    public XBrush Brush { get; set; } = XBrushes.Black;

    public RenderTitleProcessor(PageRanges pages, XPoint position)
    {
        Pages = pages;
        Position = position;
    }
    public override void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var pages = Pages.GetPages(document);

        foreach (var page in pages)
        {
            RenderTitle(page);
        }
    }

    private void RenderTitle(PdfPage page)
    {
        using var graphics = XGraphics.FromPdfPage(page);
        graphics.DrawString(page.Owner.Info.Title, Font, Brush, Position);
    }
}