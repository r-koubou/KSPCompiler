using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public static class TemplateUtil
    {
        private const string LF = "\n";

        public const int MemberIndentCount = 2;
        public const int StatementIndentCount = 3;

        public static string ExpandText<T>(
            IList<T> list,
            bool newLine,
            int indentCount = 0 )
        {
            var buffer = new StringBuilder();
            var indent = "";

            for( var i = 0; i < indentCount; i++ )
            {
                indent += "    ";
            }

            var count = list.Count;
            var index = 0;

            foreach( var x in list )
            {
                buffer.Append( $"{indent}{x}" )
                      .Append( newLine && index < count - 1 ? LF : "" );
                index++;
            }

            return buffer.ToString();
        }

        public static string ExpandTextWithField( IList<AstNodesInfo.Class.Field> fields )
        {
            var buffer = new StringBuilder();
            var indent = "";

            for( var i = 0; i < MemberIndentCount; i++ )
            {
                indent += "    ";
            }

            var index = 0;
            foreach( var f in fields )
            {
                if( !string.IsNullOrEmpty( f.Description ) )
                {
                    buffer.Append( indent ).Append( "/// <summary>" ).Append( LF );
                    buffer.Append( indent ).Append( $"/// {f.Description}" ).Append( LF );
                    buffer.Append( indent ).Append( "/// </summary>" ).Append( LF );
                }

                buffer.Append( indent )
                      .Append( f.Declaration )
                      .Append( LF );

                if( index < fields.Count - 1 )
                {
                    buffer.Append( LF );
                }

                index++;
            }

            return buffer.ToString();
        }

        public static string ExpandTextWithStatements( IList<string> statements )
        {
            return ExpandText( statements, true, StatementIndentCount );
        }

        public static string ExpandTextWithStatements( string statements )
        {
            var txt = statements.Trim();

            return string.IsNullOrEmpty( txt ) ? string.Empty
                : ExpandTextWithStatements( statements.Split( LF ) );
        }

        public static string ExpandUsing<T>( IEnumerable<T> list )
        {
            var buffer = new StringBuilder();

            foreach( var x in list )
            {
                buffer.Append( $"using {x};{LF}" );
            }

            return buffer.ToString();
        }

        public static string ExpandAttribute<T>( IList<T> list )
        {
            if( !list.Any() )
            {
                return string.Empty;
            }

            var buffer = new StringBuilder();

            buffer.Append( '[' )
                  .Append( ExpandListWithDelimiter( list, "," ) )
                  .Append( ']' );

            return buffer.ToString();
        }

        public static string ExpandTextWithEnum( IEnumerable<AstNodesInfo.Class> classes )
        {
            var ids = ( from x in classes select x.Name ).ToList();

            return ExpandListWithDelimiter<string>(
                ids,
                ",",
                false,
                true,
                true,
                MemberIndentCount
            );
        }

        public static string ExpandListWithDelimiter<T>(
            IList<T> list,
            string delimiter = "",
            bool spaceBefore = false,
            bool spaceAfter = true,
            bool newLine = false,
            int indentCount = 0 )
        {
            if( !list.Any() )
            {
                return string.Empty;
            }

            var buffer = new StringBuilder();
            var hasDelimiter = !string.IsNullOrEmpty( delimiter );

            var indent = string.Empty;

            for( int i = 0; i < indentCount; i++ )
            {
                indent += "    ";
            }

            for( var i = 0; i < list.Count; i++ )
            {
                if( newLine )
                {
                    buffer.Append( indent );
                }

                buffer.Append( list[ i ] );

                if( hasDelimiter && i < list.Count - 1 )
                {
                    buffer.Append( spaceBefore ? " " : "" );
                    buffer.Append( delimiter );
                    buffer.Append( spaceAfter ? " " : "" );
                }

                if( newLine )
                {
                    buffer.Append( LF );
                }
            }

            return buffer.ToString();
        }
    }
}