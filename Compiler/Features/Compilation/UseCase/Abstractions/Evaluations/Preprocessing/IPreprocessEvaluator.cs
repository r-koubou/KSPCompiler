using KSPCompiler.Features.Compilation.Domain.Ast.Nodes;
using KSPCompiler.Features.Compilation.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.Features.Compilation.UseCase.Abstractions.Evaluations.Preprocessing;

public interface IPreprocessEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node );
}
