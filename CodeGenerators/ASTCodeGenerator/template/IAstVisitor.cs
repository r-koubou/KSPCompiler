using __namespace__.Blocks;
using __namespace__.Expressions;
using __namespace__.Statements;

namespace __namespace__
{
    public interface IAstVisitor<out T>
    {
__visitor__

        T Visit( IAstNode node )
        {
            return Visit( (dynamic)node );
        }
    }
}
