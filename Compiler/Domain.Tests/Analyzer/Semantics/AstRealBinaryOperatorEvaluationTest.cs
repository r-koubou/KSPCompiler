using KSPCompiler.Domain.Ast.Nodes.Expressions;
using KSPCompiler.Domain.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Domain.Tests.Analyzer.Semantics;

[TestFixture]
public class AstRealBinaryOperatorEvaluationTest
{
    #region Mathmetical Operators

    [Test]
    public void AddOperatorTest()
        => MockUtility.OperatorTestBody<AstAdditionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeReal );

    [Test]
    public void SubOperatorTest()
        => MockUtility.OperatorTestBody<AstSubtractionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeReal );

    [Test]
    public void MulOperatorTest()
        => MockUtility.OperatorTestBody<AstMultiplyingExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeReal );

    [Test]
    public void DivOperatorTest()
        => MockUtility.OperatorTestBody<AstDivisionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeReal );

    #endregion ~Mathmetical Operators

    #region Not Supported Operators

    [Test]
    public void CannotModOperatorTest()
        => MockUtility.OperatorTestBody<AstModuloExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 1, DataTypeFlag.TypeReal );

    [Test]
    public void CannotBitwiseOrOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseOrExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 1, DataTypeFlag.TypeReal );

    [Test]
    public void CannotBitwiseAndOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseAndExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 1, DataTypeFlag.TypeReal );

    [Test]
    public void CannotBitwiseXorOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseXorExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 1, DataTypeFlag.TypeReal );

    #endregion ~Not Supported Operators
}
