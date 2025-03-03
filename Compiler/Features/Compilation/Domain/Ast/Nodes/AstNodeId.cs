namespace KSPCompiler.Features.Compilation.Domain.Ast.Nodes
{
    public enum AstNodeId
    {
        None,
        CompilationUnit,
        CallbackDeclaration,
        UserFunctionDeclaration,
        Modifier,
        Argument,
        ArgumentList,
        Block,
        CaseBlock,
        LogicalOr,
        LogicalAnd,
        LogicalXor,
        StringConcatenate,
        BitwiseOr,
        BitwiseAnd,
        BitwiseXor,
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessEqual,
        GreaterEqual,
        Addition,
        Subtraction,
        Multiplying,
        Division,
        Modulo,
        UnaryMinus,
        UnaryNot,
        UnaryLogicalNot,
        IntLiteral,
        RealLiteral,
        StringLiteral,
        BooleanLiteral,
        ExpressionList,
        AssignmentExpression,
        AssignmentExpressionList,
        AdditionAssignment,
        SubtractionAssignment,
        MultiplyingAssignment,
        DivisionAssignment,
        Symbol,
        ArrayElementExpression,
        CallCommandExpression,
        PreprocessorDefine,
        PreprocessorUndefine,
        PreprocessorIfdefine,
        PreprocessorIfnotDefine,
        IfStatement,
        WhileStatement,
        SelectStatement,
        CallUserFunctionStatement,
        VariableDeclaration,
        VariableInitializer,
        PrimitiveInitializer,
        ArrayInitializer,
        ContinueStatement,
    }
}
