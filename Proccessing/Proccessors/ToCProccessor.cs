using System.Text;
using PdfSharpCore.Pdf;
using Portable.Xaml;
using Portable.Xaml.Markup;

namespace PDF_TOC.Proccessing;

public class ToCProccessor : IPdfProccessor
{
    private readonly TocItems _items;

    public ToCProccessor(TocItems items)
    {
        _items = items;
    }

    public ToCProccessor(string xmldef)
    {
        _items = ToC.Load(xmldef);
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        var xml = ToC.Save(_items);
        document.CustomValues["/toc"] = new(Encoding.Default.GetBytes(xml));
        File.WriteAllText(document.Info.Title + ".toc.xml", xml);

        foreach (var item in _items)
        {
            var outline = document.Outlines.Add(item.Title, document.Pages[item.Page]);
            AddOutlineChildren(item, outline, document);
        }
    }

    private void AddOutlineChildren(TocItem item, PdfOutline parent, PdfDocument document)
    {
        foreach (var child in item.Children)
        {
            var outline = parent.Outlines.Add(child.Title, document.Pages[child.Page]);

            AddOutlineChildren(child, outline, document);
        }
    }
}

public class TocItems : List<TocItem>
{
}

[ContentProperty("Children")]
public class TocItem
{
    public string Title { get; set; }
    public int Page { get; set; }

    public TocItems Children { get; set; } = new();

    public TocItem(string title, int page)
    {
        Title = title;
        Page = page;
    }

    public TocItem(string title, int page, TocItems children)
        : this(title, page)
    {
        Children = children;
    }
}

public class ToC
{
    public static TocItems Load(string xml)
    {
        return (TocItems) XamlServices.Load(new MemoryStream(Encoding.Default.GetBytes(xml)));
    }

    public static string Save(TocItems toc)
    {
        return XamlServices.Save(toc);
    }
}