using KSPCompiler.Domain.Ast.Nodes;
using KSPCompiler.Domain.Ast.Nodes.Statements;

namespace KSPCompiler.UseCases.Analysis.Evaluations.Preprocessing;

public interface IPreprocessEvaluator
{
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorDefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorUndefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfdefineNode node );
    public IAstNode Evaluate( IAstVisitor visitor, AstPreprocessorIfnotDefineNode node );
}
