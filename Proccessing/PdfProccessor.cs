using magic.node;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public class PdfProccessor
{
    public List<PdfProcessor> Proccessors { get; set; } = new();

    public static PdfProccessor FromNode(Node node)
    {
        var processor = new PdfProccessor();
        addPdfProccessors(node, processor.Proccessors);

        return processor;
    }

    private static void addPdfProccessors(Node node, List<PdfProcessor> processors)
    {
        if (node.Value is PdfProcessor processor)
        {
            processor.Node = node;
            processors.Add(processor);
        }

        foreach (var child in node.Children)
        {
            addPdfProccessors(child, processors);
        }
    }
    
    public void Invoke()
    {
        var document = new PdfDocument();

        foreach (var proccessor in Proccessors)
        {
            proccessor.Invoke(document, this);
        }
        
        document.Save((document.Info.Title ?? "output") + ".pdf");
    }

    public T GetProccessor<T>()
        where T : PdfProcessor
    {
        return Proccessors.OfType<T>().FirstOrDefault();
    }
}