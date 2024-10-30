namespace KSPCompiler.Domain.Ast.Nodes.Extensions;

public static class AstIdExtension
{
    public static bool IsBinaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.Addition     => true,
            AstNodeId.Subtraction  => true,
            AstNodeId.Multiplying  => true,
            AstNodeId.Division     => true,
            AstNodeId.Modulo       => true,
            AstNodeId.BitwiseAnd   => true,
            AstNodeId.BitwiseOr    => true,
            AstNodeId.BitwiseXor   => true,
            AstNodeId.Equal        => true,
            AstNodeId.NotEqual     => true,
            AstNodeId.LessThan     => true,
            AstNodeId.GreaterThan  => true,
            AstNodeId.LessEqual    => true,
            AstNodeId.GreaterEqual => true,
            _                      => false
        };

    public static bool IsUnaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.UnaryNot        => true,
            AstNodeId.UnaryMinus      => true,
            AstNodeId.UnaryLogicalNot => true,
            _                         => false
        };

    public static bool IsNumericSupportedBinaryOperator( this AstNodeId id )
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

    public static bool IsNumericSupportedUnaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.UnaryNot   => true,
            AstNodeId.UnaryMinus => true,
            _                    => false
        };


    public static bool IsIntegerSupportedBinaryOperator( this AstNodeId id )
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

    public static bool IsIntegerSupportedUnaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.UnaryNot    => true,
            AstNodeId.UnaryMinus  => true,
            _                     => false
        };

    public static bool IsRealSupportedBinaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.Addition    => true,
            AstNodeId.Subtraction => true,
            AstNodeId.Multiplying => true,
            AstNodeId.Division    => true,
            _                     => false
        };

    public static bool IsRealSupportedUnaryOperator( this AstNodeId id )
        => id switch
        {
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

    public static bool IsBooleanSupportedBinaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.Equal           => true,
            AstNodeId.NotEqual        => true,
            AstNodeId.LessThan        => true,
            AstNodeId.GreaterThan     => true,
            AstNodeId.LessEqual       => true,
            AstNodeId.GreaterEqual    => true,
            _                         => false
        };

    public static bool IsBooleanSupportedUnaryOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.UnaryLogicalNot => true,
            _                         => false
        };

    public static bool IsConditionalLogicalOperator( this AstNodeId id )
        => id switch
        {
            AstNodeId.LogicalAnd => true,
            AstNodeId.LogicalOr  => true,
            AstNodeId.LogicalXor => true,
            _                    => false
        };

}
