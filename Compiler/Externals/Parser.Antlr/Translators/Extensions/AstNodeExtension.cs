using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Node;

namespace KSPCompiler.Externals.Parser.Antlr.Translators.Extensions
{
    internal static class AstNodeExtension
    {
        public static void Import( this IAstNode node, ParserRuleContext context )
        {
            node.Position = ToPosition( context );
        }

        public static void Import( this IAstNode node, ITerminalNode context )
        {
            node.Position = ToPosition( context.Symbol );
        }

        public static void Import( this IAstNode node, IToken context )
        {
            node.Position = ToPosition( context );
        }

        private static Position ToPosition( ParserRuleContext context )
        {
            return new Position()
            {
                BeginLine   = context.Start.Line,
                BeginColumn = context.Start.Column,
                EndLine     = context.Stop.Line,
                EndColumn   = context.Stop.Column
            };
        }

        public static Position ToPosition( IToken token )
        {
            return new Position()
            {
                BeginLine   = token.Line,
                BeginColumn = token.Column,
                EndLine     = LineNumber.Unknown,
                EndColumn   = Column.Unknown
            };
        }

    }
}
