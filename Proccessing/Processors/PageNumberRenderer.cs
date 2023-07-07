using PDF_TOC.Proccessing.Processors.ToC;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class PageNumberRenderer : IPdfProcessor
{
    private PdfPagePosition _position;
    private readonly string _format;
    public XFont Font { get; set; } = new ("Arial", 12);

    public PageNumberRenderer(PdfPagePosition position, string format)
    {
        _position = position;
        _format = format;
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var toCProccessor = processor.GetProccessor<ToCProcessor>();
        var tocPageCount = toCProccessor.PageCount;
        
        for (var index = 1+tocPageCount; index < document.Pages.Count; index++)
        {
            var page = document.Pages[index];

            var graphics = XGraphics.FromPdfPage(page);

            var pageNumber = index - tocPageCount;
            var documentPageCount = document.PageCount - 1 - tocPageCount;
            
            var content = _format.Replace("X", pageNumber.ToString()).Replace("Y", documentPageCount.ToString());
            var contentSize = graphics.MeasureString(content, Font);

            graphics.DrawString(content, Font,
                XBrushes.Black,
                CalculatePosition(page.Width, page.Height, contentSize));
            
            graphics.Dispose();
        }
    }

    private XPoint CalculatePosition(XUnit pageWidth, XUnit pageHeight, XSize contentSize)
    {
        var margin = 12;
        var x = _position == PdfPagePosition.BottomRight ? pageWidth.Value - contentSize.Width : contentSize.Width;
        var y = pageHeight.Value;

        return new(_position == PdfPagePosition.BottomRight ? x - margin : margin, y - margin);
    }
}

public enum PdfPagePosition
{
    BottomLeft,
    BottomRight,
}