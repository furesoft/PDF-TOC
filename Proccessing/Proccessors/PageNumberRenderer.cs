using PDF_TOC.Proccessing.Proccessors.ToC;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Proccessors;

public class PageNumberRenderer : IPdfProccessor
{
    private PdfPagePosition _position;
    private readonly string _format;

    public PageNumberRenderer(PdfPagePosition position, string format)
    {
        _position = position;
        _format = format;
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var toCProccessor = processor.GetProccessor<ToCProccessor>();
        var tocPageCount = toCProccessor.PageCount;
        
        for (var index = 1+tocPageCount; index < document.Pages.Count; index++)
        {
            var page = document.Pages[index];
            
            var graphics = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12);
            
            var pageNumber = index - tocPageCount;
            var documentPageCount = document.PageCount - 1 - tocPageCount;
            
            var content = _format.Replace("X", pageNumber.ToString()).Replace("Y", documentPageCount.ToString());
            var contentSize = graphics.MeasureString(content, font);

            graphics.DrawString(content, font,
                XBrushes.Black,
                CalculatePosition(page.Width, page.Height, contentSize));
            
            graphics.Dispose();
        }
    }

    private XPoint CalculatePosition(XUnit pageWidth, XUnit pageHeight, XSize contentSize)
    {
        int margin = 12;
        double x = _position == PdfPagePosition.BottomRight ? pageWidth.Value - contentSize.Width : contentSize.Width;
        double y = pageHeight.Value;

        return new(_position == PdfPagePosition.BottomRight ? x - margin : margin, y - margin);
    }
}

public enum PdfPagePosition
{
    BottomLeft,
    BottomRight,
}