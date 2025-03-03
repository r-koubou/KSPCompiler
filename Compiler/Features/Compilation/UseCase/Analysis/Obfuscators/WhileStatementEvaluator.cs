using System.Text;

using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;
using KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;

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
