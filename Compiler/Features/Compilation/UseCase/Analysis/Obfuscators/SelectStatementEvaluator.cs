using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Extensions;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class SelectStatementEvaluator : ISelectStatementEvaluator
{
    private StringBuilder Output { get; }

    public SelectStatementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
    {
        Output.Append( "select(" );

        statement.Condition.Accept( visitor );

        Output.Append( ')' );
        Output.NewLine();

        foreach( var caseBlock in statement.CaseBlocks )
        {
            Output.Append( "case " );

            caseBlock.ConditionFrom.Accept( visitor );

            if( caseBlock.ConditionTo.IsNotNull() )
            {
                Output.Append( " to " );
                caseBlock.ConditionTo.Accept( visitor );
            }

            Output.NewLine();

            caseBlock.CodeBlock.AcceptChildren( visitor );
        }

        Output.Append( "end select" )
              .NewLine();

        return statement;
    }
}
