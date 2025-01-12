using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Antlr4.Runtime;

using KSPCompiler.Commons.Text;

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
                EndLine     = LineNumber.Unknown,
                EndColumn   = Column.Unknown
            };
        }
        #endregion TokenPosition

        #region Comments

        private static readonly Regex RegexNewLine = new Regex( @"\r\n|\r|\n" );

        private IReadOnlyCollection<IToken> GetCommentsToLeft( ParserRuleContext context )
            => GetCommentsToLeft( context.Start.TokenIndex );

        private IReadOnlyCollection<IToken> GetCommentsToLeft( int tokenIndex )
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

        private static IReadOnlyCollection<string> GetCommentText( string commentText )
        {
            var text = commentText.Trim();

            text = text.Replace( "{", "" )
                       .Replace( "}", "" );

            return RegexNewLine.Split( text )
                               .Where( x => !string.IsNullOrWhiteSpace( x ) )
                               .ToList();
        }

        #endregion ~Comments
    }
}
