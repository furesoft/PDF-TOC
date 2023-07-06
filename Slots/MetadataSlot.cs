using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing.Processors;

namespace PDF_TOC.Slots;

[Slot(Name = "metadata")]
public class MetadataSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        var author = input.Get<string>("author");
        var title = input.Get<string>("title");

        input.Value = new MetadataProcessor(author, title);
    }
}