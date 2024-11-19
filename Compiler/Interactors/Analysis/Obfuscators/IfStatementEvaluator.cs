using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
