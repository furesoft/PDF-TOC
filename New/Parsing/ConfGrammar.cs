namespace PDF_TOC;

using Furesoft.PrattParser;
using Furesoft.PrattParser.Nodes;
using Furesoft.PrattParser.Parselets;
using static Furesoft.PrattParser.PredefinedSymbols;

public class ConfGrammar : Parser
{
    protected override void InitLexer(Lexer lexer)
    {
        lexer.MatchNumber(true, true);

        lexer.Ignore(' ');
    }

    protected override void InitParselets()
    {
        Register(Name, new FunctionCallParselet());

        Block(SOF, EOF, EOL);
        Block(LeftCurly, RightCurly, EOL);
    }
}

public class FunctionCallParselet : IPrefixParselet
{
    public AstNode Parse(Parser parser, Token token)
    {
        //name (args)? (options)?
        var name = token.Text.ToString();
        
        
    }
}
