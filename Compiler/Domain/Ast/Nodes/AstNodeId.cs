namespace KSPCompiler.Domain.Ast.Nodes
{
    public enum AstNodeId
    {
        None,
        CompilationUnit,
        CallbackDeclaration,
        UserFunctionDeclaration,
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
        IntExpression,
        RealExpression,
        StringExpression,
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
        KspPreprocessorDefine,
        KspPreprocessorUndefine,
        KspPreprocessorIfdefine,
        KspPreprocessorIfnotDefine,
        IfStatement,
        WhileStatement,
        SelectStatement,
        CallKspUserFunctionStatement,
        VariableDeclaration,
        VariableInitializer,
        PrimitiveInitializer,
        ArrayInitializer,
        ContinueStatement,
    }
}
