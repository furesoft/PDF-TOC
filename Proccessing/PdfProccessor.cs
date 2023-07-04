using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public class PdfProccessor
{
    public List<IPdfProccessor> Proccessors { get; set; } = new();

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
        where T : IPdfProccessor
    {
        return Proccessors.OfType<T>().FirstOrDefault();
    }
}