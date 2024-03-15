using magic.node;
using magic.node.extensions;
using PdfSharpCore.Drawing;

namespace PDF_TOC;

public static class NodeExtensions
{
    public static T Get<T>(this Node node, string name)
        where T : class
    {
        return node.Children.FirstOrDefault(_ => _.Name == name)?.GetEx<T>();
    }
    
    public static Node Get(this Node node, string name)
    {
        return node.Children.FirstOrDefault(_ => _.Name == name);
    }

    public static XFont GetFontByChild(this Node node, string childName = "font", double defaultSize = 12)
    {
        var child = node.Get(childName);

        var fontFamily = "Arial";
        double fontSize = defaultSize;
        
        if (child.Value is string value)
        {
            var split = value.Split(' ', StringSplitOptions.TrimEntries);
            
            fontFamily = split[0];
            if (split.Length == 2)
            {
                fontSize = double.Parse(split[1]);
            }
        }

        return new(fontFamily, fontSize);
    }

    public static XBrush GetBrushByChild(this Node node, string childName = "color", string defaultValue = "Black")
    {
        var child = node.Get(childName);

        return new XSolidBrush(XColor.FromName(child.Get<string>() ?? defaultValue));
    }

    public static bool GetFlag(this Node node, string name)
    {
        var child = node.Children.FirstOrDefault(_ => _.Name == name);

        return child.Value != null && child.Value == "true";
    }
    
    public static T Get<T>(this Node node, int index)
        where T : class
    {
        return node.Children.Skip(index).FirstOrDefault()?.GetEx<T>();
    }
}