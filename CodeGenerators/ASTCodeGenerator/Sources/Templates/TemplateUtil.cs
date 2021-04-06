using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using KSPCompiler.Apps.ASTCodeGenerator.TemplateModels;

namespace KSPCompiler.Apps.ASTCodeGenerator.Templates
{
    public static class TemplateUtil
    {
        private const string REPLACE_CLASSNAME = "##CLASSNAME##";
        private const string REPLACE_AST_ID = "##ID##";

        private const string LF = "\n";

        public const int MemberIndentCount = 2;
        public const int StatementIndentCount = 3;

        public static string MakeIndentSpace( int count )
        {
            var indent = "";

            for( var i = 0; i < count; i++ )
            {
                indent += "    ";
            }

            return indent;
        }

        public static string Replace( string text, AstNodesInfo info, AstNodesInfo.Class clazz )
        {
            var result = text;
            result = result.Replace( REPLACE_CLASSNAME, info.GetClassName( clazz ) );
            result = result.Replace( REPLACE_AST_ID,    $"AstNodeId.{clazz.Name}" );

            return result;
        }

        public static string ExpandText<T>(
            IList<T> list,
            bool newLine,
            int indentCount = 0 )
        {
            var buffer = new StringBuilder();
            var indent = MakeIndentSpace( indentCount );

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

        public static string ExpandTextWithField(
            AstNodesInfo info,
            AstNodesInfo.Class clazz,
            IList<AstNodesInfo.Class.Field> fields )
        {
            var buffer = new StringBuilder();
            var indent = MakeIndentSpace( MemberIndentCount );
            var index = 0;

            foreach( var f in fields )
            {
                if( !string.IsNullOrEmpty( f.Description ) )
                {
                    var desc = Replace( f.Description, info, clazz );
                    buffer.Append( indent ).Append( "/// <summary>" ).Append( LF );
                    buffer.Append( indent ).Append( $"/// {desc}" ).Append( LF );
                    buffer.Append( indent ).Append( "/// </summary>" ).Append( LF );
                }

                var indentedCode = f.Body.Trim().Replace( LF, $"{LF}{indent}" );

                buffer.Append( indent )
                      .Append( Replace( indentedCode, info, clazz ) )
                      .Append( LF );

                if( index < fields.Count - 1 )
                {
                    buffer.Append( LF );
                }

                index++;
            }

            return buffer.ToString();
        }

        public static string ExpandTextWithMethod(
            AstNodesInfo info,
            AstNodesInfo.Class clazz,
            IList<AstNodesInfo.Class.Method> methods )
        {
            var buffer = new StringBuilder();
            var indent = MakeIndentSpace( MemberIndentCount );
            var index = 0;

            foreach( var m in methods )
            {
                if( !string.IsNullOrEmpty( m.Description ) )
                {
                    buffer.Append( indent ).Append( "/// <summary>" ).Append( LF );
                    buffer.Append( indent ).Append( $"/// {m.Description}" ).Append( LF );
                    buffer.Append( indent ).Append( "/// </summary>" ).Append( LF );
                }

                var indentedCode = m.Body.Trim().Replace( LF, $"{LF}{indent}" );
                buffer.Append( indent ).Append( Replace( indentedCode, info, clazz ) )
                      .Append( LF );

                if( index < methods.Count - 1 )
                {
                    buffer.Append( LF );
                }

                index++;
            }

            return buffer.ToString();
        }

        public static string ExpandTextWithConstructor(
            AstNodesInfo info,
            AstNodesInfo.Class clazz,
            IList<AstNodesInfo.Class.Constructor> constructors )
        {
            var buffer = new StringBuilder();
            var indent = MakeIndentSpace( MemberIndentCount );
            var index = 0;

            foreach( var m in constructors )
            {
                ExpandTextWithMethodImpl( info, clazz, buffer, indent, "Ctor.", m.Body );

                if( index < constructors.Count - 1 )
                {
                    buffer.Append( LF );
                }

                index++;
            }

            return buffer.ToString();
        }

        public static StringBuilder ExpandTextWithMethodImpl(
            AstNodesInfo info,
            AstNodesInfo.Class clazz,
            StringBuilder buffer,
            string indent,
            string description,
            string methodBody )
        {
            if( !string.IsNullOrEmpty( description ) )
            {
                buffer.Append( indent ).Append( "/// <summary>" ).Append( LF );
                buffer.Append( indent ).Append( $"/// {description}" ).Append( LF );
                buffer.Append( indent ).Append( "/// </summary>" ).Append( LF );
            }

            var indentedCode = methodBody.Trim().Replace( LF, $"{LF}{indent}" );
            buffer.Append( indent ).Append( Replace( indentedCode, info, clazz ) )
                  .Append( LF );

            return buffer;
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

        public static string ExpandTextWithEnum( IEnumerable<AstNodesInfo.Class> classes, Func<AstNodesInfo.Class, bool> skipCond )
        {
            var ids = ( from x in classes where !skipCond(x) select x.Name ).ToList();

            return ExpandListWithDelimiter<string>(
                ids,
                ",",
                false,
                true,
                true,
                MemberIndentCount
            );
        }

        public static string ExpandTextWithEnum( IEnumerable<AstNodesInfo.Class> classes )
        {
            return ExpandTextWithEnum( classes, ( x ) => false );
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

            var indent = MakeIndentSpace( indentCount );

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