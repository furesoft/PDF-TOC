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
    public XFont ItemFont { get; set; } = new("Arial", 12);
    public XFont HeaderFont { get; set; } = new("Arial", 36);
    public int Margin { get; set; } = 12;
    public int HeaderMargin { get; set; } = 36;

    private Dictionary<TocItem, (XPoint lastItemPos, XPoint pageNumberPos, PdfPage page)> _pageNumbers = new();
    private Dictionary<PdfPage, XGraphics> _graphicsCache = new();

    private XGraphics GetGraphics(PdfPage page)
    {
        if (!_graphicsCache.ContainsKey(page))
        {
            _graphicsCache.Add(page, XGraphics.FromPdfPage(page));
        }

        return _graphicsCache[page];
    }

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
        GenerateTocPages(document);

        foreach (var item in _items)
        {
            var outline = document.Outlines.Add(item.Title, document.Pages[item.Page + PageCount]);
            AddOutlineChildren(item, outline, document);
        }

        ClearAndDisposeCache();
    }

    private void ClearAndDisposeCache()
    {
        foreach (var item in _graphicsCache)
        {
            item.Value.Dispose();
        }
        
        _graphicsCache.Clear();
    }

    private void SetTocPageMetadata(PdfPage page)
    {
        page.Tag = "TOC";
    }

    private void GenerateTocPages(PdfDocument document)
    {
        if (!_generateSeperatePage)
        {
            return;
        }

        var page = document.InsertPage(1);
        SetTocPageMetadata(page);
        var graphics = GetGraphics(page);

        XPoint lastItemPos = new(Margin, Margin);

        DrawHeader(ref lastItemPos, graphics);

        var headersize = graphics.MeasureString(Header, HeaderFont);
        var leftSpace = page.Height.Value - HeaderMargin - headersize.Height;

        GenerateItems(graphics, leftSpace, lastItemPos, page);

        CalculateActualPageCount(document);

        RenderPageNumbers(document);
    }

    private void CalculateActualPageCount(PdfDocument document)
    {
        PageCount = document.Pages.Count<PdfPage>(_ => _.Tag == "TOC");
    }

    private void GenerateItems(XGraphics graphics, double leftSpace, XPoint lastItemPos, PdfPage page,
        int startIndex = 0)
    {
        PageCount++;

        for (var index = startIndex; index < _items.Count; index++)
        {
            var item = _items[index];
            var contentSize = graphics.MeasureString(item.Title, ItemFont);

            leftSpace -= contentSize.Height;

            if (leftSpace <= Margin * 8)
            {
                if (index < _items.Count)
                {
                    page = page.Owner.InsertPage(PageCount + 1);
                    SetTocPageMetadata(page);

                    var fromPdfPage = GetGraphics(page);
                    GenerateItems(fromPdfPage, page.Height, new(Margin, Margin), page, index);
                }

                break;
            }

            var pos = new XPoint(Margin + contentSize.Width, Margin);
            lastItemPos = new(lastItemPos.X, lastItemPos.Y + pos.Y + 3);
            var pageNumberPos = GetPageNumberPos(item, graphics, lastItemPos.Y);

            _pageNumbers.TryAdd(item, (lastItemPos, pageNumberPos, page));

            graphics.DrawString(item.Title, ItemFont, XBrushes.Black, lastItemPos);
        }
    }

    private void RenderPageNumbers(PdfDocument document)
    {
        foreach (var item in _items)
        {
            var pageNumber = _pageNumbers[item];
            var graphics = GetGraphics(pageNumber.page);
            graphics.DrawString(item.Page.ToString(), ItemFont, XBrushes.Black,
                pageNumber.pageNumberPos);

            var xRect = new XRect(pageNumber.lastItemPos.X,
                pageNumber.lastItemPos.Y - Margin,
                pageNumber.page.Width.Value - 12, 25);
            var rect = graphics.Transformer.WorldToDefaultPage(xRect);

            pageNumber.page.AddDocumentLink(new(rect), item.Page + PageCount + 1);
        }
    }

    private void DrawHeader(ref XPoint lastItemPos, XGraphics graphics)
    {
        var headersize = graphics.MeasureString(Header, HeaderFont);

        graphics.DrawString(Header, HeaderFont, XBrushes.Black, new XPoint(Margin, Margin + headersize.Height));

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