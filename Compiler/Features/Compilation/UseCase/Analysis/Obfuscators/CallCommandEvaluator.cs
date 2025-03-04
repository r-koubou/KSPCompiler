using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Commands;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Ast.Nodes;
using KSPCompiler.Shared.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Ast.Nodes.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class CallCommandEvaluator : ICallCommandEvaluator
{
    private StringBuilder Output { get; }
    public CallCommandEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstCallCommandExpressionNode node )
    {
        node.Left.Accept( visitor );

        if( node.Right.IsNotNull() )
        {
            Output.Append( '(' );
        }

        if( node.Right is AstExpressionListNode expressionList )
        {
            Output.AppendExpressionList( visitor, expressionList );
        }

        if( node.Right.IsNotNull() )
        {
            Output.Append( ')' );
        }

        /*
            このコマンド呼び出しが式中ではなくステートメントの場合は改行する
            => コールバックや関数内のステートメントリストの直下のノード

            例：
            on init
                commandA()              <-- command A is statement (append newline)
                $x := commandA()        <-- command A is statement (append newline)
                commandB( commandA() )  <-- command A is expression (NOT append newline)
            end on
        */
        if( node.Parent is AstBlockNode )
        {
            Output.NewLine();
        }

        return node;
    }
}
