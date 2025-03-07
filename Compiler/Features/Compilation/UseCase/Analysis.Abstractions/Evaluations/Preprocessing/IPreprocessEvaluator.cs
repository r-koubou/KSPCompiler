using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions.Evaluations.Preprocessing;

public interface IPreprocessEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node );
}
