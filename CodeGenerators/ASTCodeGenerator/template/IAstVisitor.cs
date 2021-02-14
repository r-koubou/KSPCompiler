namespace Ast
{
    public interface IAstVisitor<T>
    {
__visitor__

        T Visit( IAstNode node )
        {
            return Visit( (dynamic)node );
        }
    }
}
