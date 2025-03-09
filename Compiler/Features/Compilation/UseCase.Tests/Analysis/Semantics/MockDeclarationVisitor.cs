using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Declarations;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.SymbolManagement.UseCase.Tests.Analysis.Semantics;

public class MockDeclarationVisitor : DefaultAstVisitor
{
    private IVariableDeclarationEvaluator VariableDeclarationEvaluator { get; set; } = new MockVariableDeclarationEvaluator();
    private ICallbackDeclarationEvaluator CallbackDeclarationEvaluator { get; set; } = new MockCallbackDeclarationEvaluator();
    private IUserFunctionDeclarationEvaluator UserFunctionDeclarationEvaluator { get; set; } = new MockUserFunctionDeclarationEvaluator();

    public void Inject( IVariableDeclarationEvaluator variableDeclarationEvaluator )
    {
        VariableDeclarationEvaluator = variableDeclarationEvaluator;
    }

    public void Inject( ICallbackDeclarationEvaluator callbackDeclarationEvaluator )
    {
        CallbackDeclarationEvaluator = callbackDeclarationEvaluator;
    }

    public void Inject( IUserFunctionDeclarationEvaluator userFunctionDeclarationEvaluator )
    {
        UserFunctionDeclarationEvaluator = userFunctionDeclarationEvaluator;
    }

    public override IAstNode Visit( AstCallbackDeclarationNode node )
        => CallbackDeclarationEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => UserFunctionDeclarationEvaluator.Evaluate( this, node );

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => VariableDeclarationEvaluator.Evaluate( this, node );
}
