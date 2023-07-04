using System.Diagnostics;
using System.Text;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Portable.Xaml;
using Portable.Xaml.Markup;

namespace PDF_TOC.Proccessing;

public class ToCProccessor : IPdfProccessor
{
    public int PageCount { get; private set; }
    public string Header { get; set; }

    private readonly TocItems _items;
    private readonly bool _generateSeperatePage;
    public XFont ItemFont { get; set; } = new ("Arial", 12);
    public XFont HeaderFont { get; set; } = new ("Arial", 36);
    public int Margin { get; set; } = 12;
    public int HeaderMargin { get; set; } = 36;

    public ToCProccessor(TocItems items, bool generateSeperatePage = true, string header = "Table of Contents")
    {
        _items = items;
        _generateSeperatePage = generateSeperatePage;
        Header = header;
    }

    public ToCProccessor(string xmldef)
    {
        _items = ToC.Load(xmldef);
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        GenerateTocPage(document);

        var xml = ToC.Save(_items);
        document.CustomValues["/toc"] = new(Encoding.Default.GetBytes(xml));
        File.WriteAllText(document.Info.Title + ".toc.xml", xml);

        foreach (var item in _items)
        {
            var outline = document.Outlines.Add(item.Title, document.Pages[item.Page + PageCount]);
            AddOutlineChildren(item, outline, document);
        }
    }

    private void GenerateTocPage(PdfDocument document)
    {
        if (!_generateSeperatePage)
        {
            return;
        }

        var page = document.InsertPage(1);
        using var graphics = XGraphics.FromPdfPage(page);
        
        XPoint lastItemPos = new(Margin,Margin);

        DrawHeader(ref lastItemPos, graphics);

        var headersize = graphics.MeasureString(Header, HeaderFont);
        var leftSpace = page.Height.Value - HeaderMargin - headersize.Height;

        GenerateItems(graphics, leftSpace, lastItemPos, page);
    }

    private void GenerateItems(XGraphics graphics, double leftSpace, XPoint lastItemPos, PdfPage page, int startIndex = 0)
    {
        PageCount++;
        
        for (var index = startIndex; index < _items.Count; index++)
        {
            var item = _items[index];
            var actualPage = item.Page - PageCount;
            var contentSize = graphics.MeasureString(item.Title, ItemFont);

            leftSpace -= contentSize.Height;
            
            if (leftSpace <= Margin * 8)
            {
                if (index < _items.Count)
                {
                    page = page.Owner.InsertPage(PageCount + 1);
                    
                    GenerateItems(XGraphics.FromPdfPage(page), page.Height, new(Margin, Margin), page, index);
                }
                
                break;
            }
            
            var pos = new XPoint(Margin + contentSize.Width, Margin);
            lastItemPos = new(lastItemPos.X, lastItemPos.Y + pos.Y + 3);
            var pageNumberPos = GetPageNumberPos(item, graphics, lastItemPos.Y);

            graphics.DrawString(item.Title, ItemFont, XBrushes.Black, lastItemPos);
            graphics.DrawString(actualPage.ToString(), ItemFont, XBrushes.Black,
                pageNumberPos);

            var rect = graphics.Transformer.WorldToDefaultPage(new(lastItemPos.X, lastItemPos.Y - pos.Y,
                page.Width.Value - 12, 25));
            page.AddDocumentLink(new(rect), actualPage + PageCount + 2);
        }
    }

    private void DrawHeader(ref XPoint lastItemPos, XGraphics graphics)
    {
        var headersize = graphics.MeasureString(Header, HeaderFont);
        
        graphics.DrawString(Header, HeaderFont, XBrushes.Black, new XPoint(Margin,Margin + headersize.Height));

        lastItemPos = new(Margin, headersize.Height + HeaderMargin);
    }

    private XPoint GetPageNumberPos(TocItem item, XGraphics graphics, double y)
    {
        var actualPage = item.Page - PageCount - 1;
        var actualPageSize = graphics.MeasureString(actualPage.ToString(), ItemFont);
        
        return new(graphics.PageSize.Width - actualPageSize.Width - Margin, y);
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