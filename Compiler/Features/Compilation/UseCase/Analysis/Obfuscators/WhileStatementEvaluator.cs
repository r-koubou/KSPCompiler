using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

public class WhileStatementEvaluator : IWhileStatementEvaluator
{
    private StringBuilder Output { get; }

    public WhileStatementEvaluator( StringBuilder output )
    {
        Output = output;
    }

    public IAstNode Evaluate( IAstVisitor visitor, AstWhileStatementNode statement )
    {
        Output.Append( "while(" );

        statement.Condition.Accept( visitor );

        Output.Append( ')' );
        Output.NewLine();

        statement.CodeBlock.AcceptChildren( visitor );

        Output.Append( "end while" );
        Output.NewLine();

        return statement;
    }
}
