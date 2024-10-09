namespace KSPCompiler.Domain.Ast.Nodes.Extensions;

public static class AstIdExtension
{
    public static bool IsBinaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.Addition    => true,
            AstNodeId.Subtraction => true,
            AstNodeId.Multiplying => true,
            AstNodeId.Division    => true,
            AstNodeId.Modulo      => true,
            AstNodeId.BitwiseAnd  => true,
            AstNodeId.BitwiseOr   => true,
            AstNodeId.BitwiseXor  => true,
            _                     => false
        };

    public static bool IsUnaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.UnaryNot   => true,
            AstNodeId.UnaryMinus => true,
            _                    => false
        };

    public static bool IsBooleanOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.Equal           => true,
            AstNodeId.NotEqual        => true,
            AstNodeId.LessThan        => true,
            AstNodeId.GreaterThan     => true,
            AstNodeId.LessEqual       => true,
            AstNodeId.GreaterEqual    => true,
            AstNodeId.UnaryLogicalNot => true,
            _                         => false
        };

    public static bool IsConditionalOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.LogicalAnd => true,
            AstNodeId.LogicalOr  => true,
            AstNodeId.LogicalXor => true,
            _                    => false
        };
}
