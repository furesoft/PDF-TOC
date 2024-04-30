namespace PDF_TOC.Proccessing.Processors.ToC;

public class TocItem
{
    public string Title { get; set; }
    public int Page { get; set; }

    public List<TocItem> Children { get; set; } = [];

    public TocItem(string title, int page)
    {
        Title = title;
        Page = page;
    }

    public TocItem(string title, int page, List<TocItem> children)
        : this(title, page)
    {
        Children = children;
    }
}