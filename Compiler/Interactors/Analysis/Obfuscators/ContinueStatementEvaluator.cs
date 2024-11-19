using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ContinueStatementEvaluator : IContinueStatementEvaluator
{
    private StringBuilder Output { get; }

    public ContinueStatementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstContinueStatementNode statement )
    {
        Output.Append( "continue" )
              .NewLine();

        return statement;
    }
}
