using magic.node;
using magic.signals.contracts;
using PDF_TOC.Proccessing.Processors.ToC;

namespace PDF_TOC.Slots;

[Slot(Name = "table-of-contents")]
public class ToCSlot : ISlot
{
    public void Signal(ISignaler signaler, Node input)
    {
        var header = input.Value.ToString() ?? "Table of Contents";

        var items = new List<TocItem>();
        ConvertToItems(input.Children, items);

        input.Value = new ToCProcessor(items, header: header);
    }

    private static void ConvertToItems(IEnumerable<Node> nodeChildren, List<TocItem> items)
    {
        var children = new List<TocItem>();
        
        foreach (var child in nodeChildren)
        {
            var page = int.Parse(child.Value.ToString());
            
            items.Add(new(child.Name, page));
            
            ConvertToItems(child.Children, children);
        }
    }
}