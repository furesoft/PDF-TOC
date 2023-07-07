using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing;
using PDF_TOC.Proccessing.Processors;
using PdfSharpCore.Drawing;

namespace PDF_TOC.Slots;

[Slot(Name = "render-title")]
public class RenderTitleSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        var pages = PageRanges.Parse(input.Value.ToString());
        var position = XPoint.Parse(input.Get<string>("position")); 
        var brush = new XSolidBrush(XColor.FromName(input.Get<string>("brush") ?? "Black"));
        
        var processor = new RenderTitleProcessor(pages, position);
        processor.Brush = brush;
        
        input.Value = processor;
    }
}