using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors.ToC;

public class ToCProcessor : IPdfProcessor
{
    public int PageCount { get; private set; }
    public string Header { get; set; }

    private readonly List<TocItem> _items;
    private readonly bool _generateSeperatePage;
    public XFont ItemFont { get; set; } = new("Arial", 12);
    public XFont HeaderFont { get; set; } = new("Arial", 36);
    public int Margin { get; set; } = 12;
    public int HeaderMargin { get; set; } = 36;

    private readonly Dictionary<TocItem, (XPoint lastItemPos, XPoint pageNumberPos, PdfPage page)> _pageNumbers = new();
    private readonly Dictionary<PdfPage, XGraphics> _graphicsCache = new();

    private XGraphics GetGraphics(PdfPage page)
    {
        if (!_graphicsCache.ContainsKey(page))
        {
            _graphicsCache.Add(page, XGraphics.FromPdfPage(page));
        }

        return _graphicsCache[page];
    }

    public ToCProcessor(List<TocItem> items, bool generateSeperatePage = true, string header = "Table of Contents")
    {
        _items = items;
        _generateSeperatePage = generateSeperatePage;
        Header = header;
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

    private static void SetTocPageMetadata(PdfPage page)
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

        RenderPageNumbers();
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
                    GenerateItems(fromPdfPage, page.Height- Margin, new(Margin, Margin), page, index);
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

    private void RenderPageNumbers()
    {
        foreach (var item in _items)
        {
            var pageNumber = _pageNumbers[item];
            var graphics = GetGraphics(pageNumber.page);
            graphics.DrawString(item.Page.ToString(), ItemFont, XBrushes.Black, pageNumber.pageNumberPos);

            var documentRect = CalculateDocumentLinkRectangle(pageNumber.lastItemPos, pageNumber.page, graphics);
            pageNumber.page.AddDocumentLink(new(documentRect), item.Page + PageCount + 1);
        }
    }

    private XRect CalculateDocumentLinkRectangle(XPoint lastItemPos, PdfPage page, XGraphics graphics)
    {
        var y = lastItemPos.Y - Margin;
        var xRect = new XRect(lastItemPos.X, y, page.Width.Value - 12, 25);

        return graphics.Transformer.WorldToDefaultPage(xRect);
    }

    private void DrawHeader(ref XPoint lastItemPos, XGraphics graphics)
    {
        var headersize = graphics.MeasureString(Header, HeaderFont);

        graphics.DrawString(Header, HeaderFont, XBrushes.Black, new XPoint(Margin, Margin + headersize.Height));

        lastItemPos = new(Margin, headersize.Height + HeaderMargin);
    }

    private XPoint GetPageNumberPos(TocItem item, XGraphics graphics, double y)
    {
        var actualPage = item.Page - PageCount;
        var text = actualPage.ToString();
        var pageNumberWidth = graphics.MeasureString(text, ItemFont).Width;

        return new(graphics.PageSize.Width - pageNumberWidth - Margin, y);
    }

    private static void AddOutlineChildren(TocItem item, PdfOutline parent, PdfDocument document)
    {
        foreach (var child in item.Children)
        {
            var outline = parent.Outlines.Add(child.Title, document.Pages[child.Page]);

            AddOutlineChildren(child, outline, document);
        }
    }
}