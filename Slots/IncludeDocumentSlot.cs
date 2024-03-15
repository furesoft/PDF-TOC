using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing;
using PDF_TOC.Proccessing.Processors;

namespace PDF_TOC.Slots;

[Slot(Name = "include-document")]
public class IncludeDocumentSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        var rangeNode = input.Get("pages");
        var includeToC = input.GetFlag("toc");

        if (rangeNode != null)
        {
            var range = PageRanges.Parse(rangeNode.Value.ToString());
            input.Value = new DocumentInclude(input.Value.ToString(), range, includeToC);
        }

        input.Value = new DocumentInclude(input.Value.ToString(), includeToC);
    }
}