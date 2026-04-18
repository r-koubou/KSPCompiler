namespace KSPCompiler.Shared.Domain.Compilation.Ast.Nodes;

public enum AstNodeId
{
    None,{% for name in names %}
    {{ name }}{{ "," if not loop.last else "" }}{% endfor %}
}
