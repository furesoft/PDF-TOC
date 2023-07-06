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
        var rangeNode = input.Children.FirstOrDefault(_ => _.Name == "pages");
        if (rangeNode != null)
        {
            var range = PageRanges.Parse(rangeNode.Value.ToString());
            input.Value = new DocumentInclude(input.Value.ToString(), range);
        }

        input.Value = new DocumentInclude(input.Value.ToString());
    }
}