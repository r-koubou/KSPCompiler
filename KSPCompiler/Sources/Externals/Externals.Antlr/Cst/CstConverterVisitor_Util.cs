using Antlr4.Runtime;

using KSPCompiler.Domain.Ast;
using KSPCompiler.Domain.TextFile.Aggregate;
using KSPCompiler.Externals.Antlr.Generated;

namespace KSPCompiler.Externals.Antlr.Cst
{
    internal partial class CstConverterVisitor : KSPParserBaseVisitor<AstNode>
    {
        #region TokenPosition
        private static Position ToPosition( ParserRuleContext context )
        {
            return new ()
            {
                BeginLine = context.Start.Line,
                BeginColumn = context.Start.Column,
                EndLine = context.Stop.Line,
                EndColumn = context.Stop.Column
            };
        }

        public static Position ToPosition( IToken token )
        {
            return new ()
            {
                BeginLine = token.Line,
                BeginColumn = token.Column,
                EndLine = -1,
                EndColumn = -1
            };
        }
        #endregion TokenPosition
    }
}
