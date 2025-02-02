using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Antlr4.Runtime;

using KSPCompiler.Commons.Text;
using KSPCompiler.Domain.Ast.Nodes;

// ReSharper disable UnusedMember.Local

namespace KSPCompiler.Infrastructures.Parser.Antlr.Translators
{
    public partial class CstConverterVisitor
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
                EndLine     = token.Line,
                EndColumn   = token.Column + token.Text.Length
            };
        }

        public static Position ToPosition( IToken beginToken, IToken endToken )
        {
            return new Position
            {
                BeginLine   = beginToken.Line,
                BeginColumn = beginToken.Column,
                EndLine     = endToken.Line,
                EndColumn   = endToken.Column
            };
        }

        public static void SetFunctionalPosition( AstFunctionalNode node, IToken beginOn, IToken name, IToken end, IToken endOn )
        {
            node.BeginOnKeywordPosition = ToPosition( beginOn );
            node.NamePosition           = ToPosition( name );
            node.EndKeywordPosition     = ToPosition( end );
            node.EndOnKeywordPosition   = ToPosition( endOn );
        }
        #endregion TokenPosition

        #region Comments
        private static readonly Regex RegexNewLine = new Regex( @"\r\n|\r|\n" );

        private IReadOnlyCollection<IToken> GetCommentTokensToLeft( ParserRuleContext context )
            => GetCommentTokensToLeft( context.Start.TokenIndex );

        private IReadOnlyCollection<IToken> GetCommentTokensToLeft( int tokenIndex )
        {
            var comments = new List<IToken>();

            for( var i = tokenIndex - 1; i >= 0; i-- )
            {
                var token = tokenStream.Get( i );

                if( token.Type is KSPLexer.EOL or KSPLexer.MULTI_LINE_DELIMITER )
                {
                    continue;
                }
                else if( token.Channel == KSPLexer.COMMENT_CHANNEL )
                {
                    comments.Add( token );
                }
                else if( token.Channel != Lexer.DefaultTokenChannel )
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            comments.Reverse();

            return comments;
        }

        private IReadOnlyCollection<string> GetCommentTextLinesToLeft( ParserRuleContext context )
            => GetCommentTextLinesToLeft( context.Start.TokenIndex );

        private IReadOnlyCollection<string> GetCommentTextLinesToLeft( int tokenIndex )
        {
            var tokens = GetCommentTokensToLeft( tokenIndex );

            var result = new List<string>();

            if( tokens.Any() )
            {
                result.AddRange( GetCommentText( tokens.Last().Text ) );
            }

            return result;
        }

        private static IReadOnlyCollection<string> GetCommentText( string commentText )
        {
            var text = commentText.Trim();

            text = text.Replace( "{", "" )
                       .Replace( "}", "" );

            return RegexNewLine.Split( text ).ToList();
        }
        #endregion ~Comments
    }
}
