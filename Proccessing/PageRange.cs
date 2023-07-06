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
}

public class PageRange
{
    public int Start { get; set; }
    public int End { get; set; }

    public static PageRange All()
    {
        return new() { Start = 0, End = 0 };
    }

    public IEnumerable<int> GetIndices()
    {
        return Enumerable.Range(Start, End - Start);
    }

    internal static PageRange Parse(string src)
    {
        if (src == "*")
        {
            return All();
        }

        if (src.Contains('-'))
        {
            var splitted = src.Split('-');
            var start = splitted[0];
            var end = splitted[1];

            return new() { Start = int.Parse(start)-1, End = int.Parse(end)-1 };
        }

        return int.TryParse(src, out var pageIndex) ? new(){ Start = pageIndex, End = pageIndex} : All();
    }

    public static implicit operator PageRanges(PageRange pr)
    {
        return new(){pr};
    }
}