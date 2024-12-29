using System.Linq;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators.Extensions
{
    internal static class AstNodeExtension
    {
        public static void Import( this IAstNode node, BufferedTokenStream tokenStream, ParserRuleContext context )
        {
            node.Position = ToPosition( tokenStream, context );
        }

        public static void Import( this IAstNode node, BufferedTokenStream tokenStream, ITerminalNode context )
        {
            node.Position = ToPosition( tokenStream, context.Symbol );
        }

        public static void Import( this IAstNode node, BufferedTokenStream tokenStream, IToken token )
        {
            node.Position = ToPosition( tokenStream, token );
        }

        private static Position ToPosition( BufferedTokenStream tokenStream, ParserRuleContext context )
        {
            // hiddenチャネルのトークン（空白文字）を取得
            // var startHiddenTokens = tokenStream.GetHiddenTokensToLeft( context.Start.TokenIndex );
            var stopHiddenTokens = tokenStream.GetHiddenTokensToRight( context.Stop.TokenIndex );

            // トークンの長さを計算
            // var leadingCount = startHiddenTokens?.Sum( x => x.Text.Length ) ?? 0;
            var trailingCount = stopHiddenTokens?.Sum( x => x.Text.Length ) ?? 0;

            // 列情報を計算
            // var beginColumn = context.Start.Column + leadingCount;
            var beginColumn = context.Start.Column;
            var endColumn = context.Stop.Column + ( context.Stop.Text?.Length ?? 0 ) + trailingCount;

            // 改行を跨ぐ場合の補正
            if( context.Start.Line != context.Stop.Line )
            {
                endColumn = context.Stop.Column + ( context.Stop.Text?.Length ?? 0 );
                //endColumn = context.Stop.Text?.Length ?? context.Stop.Column;
            }

            return new Position()
            {
                BeginLine   = context.Start.Line,
                BeginColumn = beginColumn,
                EndLine     = context.Stop.Line,
                EndColumn   = endColumn
            };
        }

        private static Position ToPosition( BufferedTokenStream tokenStream, IToken token )
        {
            // 隠しトークンを取得（context.Startの直前とcontext.Stopの直後）
            var startHiddenTokens = tokenStream.GetHiddenTokensToLeft( token.TokenIndex );

            // 隠しトークンの長さを計算
            var leadingCount = startHiddenTokens?.Sum( x => x.Text.Length ) ?? 0;

            // 列情報を計算
            var beginColumn = token.Column + leadingCount;

            return new Position()
            {
                BeginLine   = token.Line,
                BeginColumn = beginColumn,
                EndLine     = LineNumber.Unknown,
                EndColumn   = Column.Unknown
            };
        }
    }
}
