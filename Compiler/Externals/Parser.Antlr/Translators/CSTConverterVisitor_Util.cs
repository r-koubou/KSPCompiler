using Antlr4.Runtime;

using KSPCompiler.Commons.Text;

// ReSharper disable UnusedMember.Local

namespace KSPCompiler.Externals.Parser.Antlr.Translators
{
    public partial class CSTConverterVisitor
    {
        #region TokenPosition
        private static Position ToPosition( ParserRuleContext context )
        {
            return new Position
            {
                BeginLine   = context.Start.Line,
                BeginColumn = context.Start.Column,
                EndLine     = context.Stop.Line,
                EndColumn   = context.Stop.Column
            };
        }

        public static Position ToPosition( IToken token )
        {
            return new Position
            {
                BeginLine   = token.Line,
                BeginColumn = token.Column,
                EndLine     = LineNumber.Unknown,
                EndColumn   = Column.Unknown
            };
        }
        #endregion TokenPosition
    }
}