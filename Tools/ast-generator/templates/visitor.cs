namespace {{ namespace }};

using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Blocks;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Statements;

public interface IAstVisitor<out T>
{
    #region Generated Code
    {% for name in class_names %}
    T Visit( {{ name }}Node node);{% endfor %}

    #endregion ~Generated Code

    /// <summary>
    /// Non-generic version of <see cref="IAstVisitor{T}"/>. <see cref="IAstNode" /> is used as the return type.
    /// </summary>
    public interface IAstVisitor : IAstVisitor<IAstNode> {}
}
