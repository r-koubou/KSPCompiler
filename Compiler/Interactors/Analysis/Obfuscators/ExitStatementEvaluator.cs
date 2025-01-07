using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Statements;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

public class ExitStatementEvaluator : IExitStatementEvaluator
{
    private StringBuilder Output { get; }

    public ExitStatementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstExitStatementNode statement )
    {
        Output.Append( "exit" )
              .NewLine();

        return statement;
    }
}
