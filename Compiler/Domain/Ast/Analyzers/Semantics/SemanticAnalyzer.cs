using KSPCompiler.Domain.Ast.Analyzers.Context;
using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Domain.Ast.Analyzers.Semantics;

public partial class SemanticAnalyzer : DefaultAstVisitor, IAstTraversal
{
    public IAnalyzerContext Context { get; }

    public SemanticAnalyzer( IAnalyzerContext context )
    {
        Context = context;
    }

    public void Traverse( AstCompilationUnitNode node )
    {
        node.AcceptChildren( this );
    }

    #region Declarations

    public override IAstNode Visit( AstCallbackDeclarationNode node )
        => Context.DeclarationContext.Callback.Evaluate( this, node );

    public override IAstNode Visit( AstUserFunctionDeclarationNode node )
        => Context.DeclarationContext.UserFunction.Evaluate( this, node );

    public override IAstNode Visit( AstVariableDeclarationNode node )
        => Context.DeclarationContext.Variable.Evaluate( this, node );

    #endregion ~Declarations



    #region Statements

    #region Preprocessor Symbol Statements

    public override IAstNode Visit( AstPreprocessorIfdefineNode node )
        => Context.StatementContext.Preprocess.Evaluate( this, node );

    public override IAstNode Visit( AstPreprocessorIfnotDefineNode node )
        => Context.StatementContext.Preprocess.Evaluate( this, node );

    #endregion ~Preprocessor Symbol Statements

    #region Call User Function Statements

    public override IAstNode Visit( AstCallUserFunctionStatementNode node )
        => Context.StatementContext.CallUserFunction.Evaluate( this, node );

    #endregion ~Call User Function Statements

    #region Control Statements

    public override IAstNode Visit( AstIfStatementNode node )
        => Context.StatementContext.If.Evaluate( this, node );

    public override IAstNode Visit( AstWhileStatementNode node )
        => Context.StatementContext.While.Evaluate( this, node );

    public override IAstNode Visit( AstSelectStatementNode node )
        => Context.StatementContext.Select.Evaluate( this, node );

    public override IAstNode Visit( AstContinueStatementNode node )
        => Context.StatementContext.Continue.Evaluate( this, node );

    #endregion ~Control Statements

    #endregion ~Statements

}
