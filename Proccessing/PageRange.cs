using System.ComponentModel;
using System.Globalization;

namespace PDF_TOC.Proccessing;

public class PageRange
{
    public int Start { get; set; }
    public int End { get; set; }

    public static PageRange All()
    {
        return new PageRange() { Start = 0, End = 0 };
    }

    public IEnumerable<int> GetIndices()
    {
        return Enumerable.Range(Start, End - Start);
    }

    public static PageRange Parse(string src)
    {
        if (src == "*")
        {
            return PageRange.All();
        }

        if (src.Contains('-'))
        {
            var splitted = src.Split('-');
            var start = splitted[0];
            var end = splitted[1];

            return new PageRange() { Start = int.Parse(start)-1, End = int.Parse(end)-1 };
        }

        return int.TryParse(src, out var pageIndex) ? new(){ Start = pageIndex, End = pageIndex} : PageRange.All();
    }
}