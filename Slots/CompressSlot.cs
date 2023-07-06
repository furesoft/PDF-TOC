using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing.Processors;

namespace PDF_TOC.Slots;

[Slot(Name = "compress")]
public class CompressSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        input.Value = new CompressionProcessor();
    }
}