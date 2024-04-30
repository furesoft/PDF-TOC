using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing;

public class PageRanges : List<PageRange>
{
    public static PageRanges Parse(string ranges)
    {
        var result = new PageRanges();
        var spl = ranges.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var range in spl)
        {
            result.Add(PageRange.Parse(range));
        }
        
        return result;
    }

    public IEnumerable<PdfPage> GetPages(PdfDocument document)
    {
        foreach (var range in this)
        {
            var indices = range.GetIndices();
            
            foreach (var index in indices)
            {
                yield return document.Pages[index];
            }
        }
    }
}

public class PageRange
{
    public int Start { get; set; }
    public int End { get; set; }

    public static PageRange All()
    {
        return new() { Start = 0, End = 0 };
    }

    public static PageRange Single(int pageIndex) 
    {
        return new() { Start = pageIndex, End = pageIndex };
    }

    public IEnumerable<int> GetIndices()
    {
        return Enumerable.Range(Start, End - Start + 1).Select(_=> _ - 1);
    }

    internal static PageRange Parse(string src)
    {
        if (src == "*")
        {
            return All();
        }

        if (!src.Contains('-'))
        {
            return int.TryParse(src, out var pageIndex) ? Single(pageIndex) : All();
        }
        
        var splitted = src.Split('-');
        var start = splitted[0];
        var end = splitted[1];

        return new() { Start = int.Parse(start)-1, End = int.Parse(end) - 1 };
    }

    public static implicit operator PageRanges(PageRange pr)
    {
        return [pr];
    }
}