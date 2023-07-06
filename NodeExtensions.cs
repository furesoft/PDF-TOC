using magic.node;
using magic.node.extensions;

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
    
    public static T Get<T>(this Node node, int index)
        where T : class
    {
        return node.Children.Skip(index).FirstOrDefault()?.GetEx<T>();
    }
}