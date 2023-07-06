using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing.Processors;

namespace PDF_TOC.Slots;

[Slot(Name = "page-numbers")]
public class PageNumberSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        var positionString = input.Get<string>("position");

        var position = positionString == null ? PdfPagePosition.BottomRight 
            : Enum.Parse<PdfPagePosition>(positionString, true);

        var format = input.Get<string>("format") ?? input.Value.ToString();

        input.Value = new PageNumberRenderer(position, format);
    }
}