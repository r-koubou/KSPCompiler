using System.Text;

using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;
using KSPCompiler.Interactors.Analysis.Obfuscators.Extensions;
using KSPCompiler.UseCases.Analysis.Evaluations.Conditionals;

namespace KSPCompiler.Interactors.Analysis.Obfuscators;

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
