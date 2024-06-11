using Furesoft.PrattParser.Nodes;

namespace PDF_TOC;

public class FunctionCallNode : AstNode
{
    public string Name { get; set; }
    public BlockNode Options { get; set; }
    public List<AstNode> Args { get; set; } = [];
}