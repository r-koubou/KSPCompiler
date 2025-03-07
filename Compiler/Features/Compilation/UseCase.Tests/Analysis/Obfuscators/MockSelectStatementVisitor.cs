using System;
using System.Text;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Statements;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators.Extensions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Obfuscators;

public class MockSelectStatementVisitor : DefaultAstVisitor
{
    private StringBuilder Ooutput { get; }
    private ISelectStatementEvaluator Evaluator { get; set; } = new MockSelectStatementEvaluator();

    public MockSelectStatementVisitor( StringBuilder ooutput )
    {
        Ooutput = ooutput;
    }

    public void Inject( ISelectStatementEvaluator evaluator )
    {
        Evaluator = evaluator;
    }

    public override IAstNode Visit( AstSelectStatementNode node )
        => Evaluator.Evaluate( this, node );

    public override IAstNode Visit( AstSymbolExpressionNode node )
    {
        Ooutput.Append( node.Name );
        return node;
    }

    public override IAstNode Visit( AstIntLiteralNode node )
    {
        Ooutput.Append( node.Value );
        return node;
    }

    public override IAstNode Visit( AstCallCommandExpressionNode node )
    {
        node.Left.Accept( this );

        Ooutput.Append( "()" )
               .NewLine();

        return node;
    }

    private class MockSelectStatementEvaluator : ISelectStatementEvaluator
    {
        public IAstNode Evaluate( IAstVisitor visitor, AstSelectStatementNode statement )
            => throw new NotImplementedException();
    }
}
