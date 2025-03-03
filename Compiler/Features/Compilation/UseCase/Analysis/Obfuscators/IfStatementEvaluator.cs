using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class IfStatementEvaluator : IIfStatementEvaluator
{
    private StringBuilder Output { get; }

    public IfStatementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstIfStatementNode statement )
    {
        Output.Append( "if(" );

        statement.Condition.Accept( visitor );;

        Output.Append( ')' );
        Output.NewLine();

        statement.CodeBlock.AcceptChildren( visitor );

        if( statement.ElseBlock.Statements.Count > 0 )
        {
            Output.Append( "else" );
            Output.NewLine();
            statement.ElseBlock.AcceptChildren( visitor );
        }

        Output.Append( "end if" );
        Output.NewLine();

        return statement;
    }
}
