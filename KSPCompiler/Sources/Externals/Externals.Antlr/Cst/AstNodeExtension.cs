using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.TextFile.Aggregate;

namespace KSPCompiler.Externals.Antlr.Cst
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
            return new()
            {
                BeginLine   = context.Start.Line,
                BeginColumn = context.Start.Column,
                EndLine     = context.Stop.Line,
                EndColumn   = context.Stop.Column
            };
        }

        public static Position ToPosition( IToken token )
        {
            return new()
            {
                BeginLine   = token.Line,
                BeginColumn = token.Column,
                EndLine     = -1,
                EndColumn   = -1
            };
        }

    }
}
