using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Expressions;

namespace KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder NewLine( this StringBuilder self )
    {
        return self.Append( ObfuscatorConstants.NewLine );
    }

    public static StringBuilder AppendUnaryOperator( this StringBuilder self, IAstVisitor visitor, string op, IAstNode left )
    {
        self.Append( $" {op} " );
        left.Accept( visitor );

        return self;
    }

    public static StringBuilder AppendBinaryOperator( this StringBuilder self, IAstVisitor visitor, string op, IAstNode left, IAstNode right )
    {
        left.Accept( visitor );
        self.Append( $" {op} " );
        right.Accept( visitor );

        return self;
    }

    public static StringBuilder AppendExpressionList( this StringBuilder self, IAstVisitor visitor, AstExpressionListNode node )
    {
        for( var i = 0; i < node.Expressions.Count; i++ )
        {
            node.Expressions[ i ].Accept( visitor );

            if( i < node.Expressions.Count - 1 )
            {
                self.Append( ", " );
            }
        }

        return self;
    }
}
