using KSPCompiler.Shared.Domain.Compilation.Ast.Nodes.Expressions;
using KSPCompiler.Shared.Domain.Compilation.Symbols.MetaData;

using NUnit.Framework;

namespace KSPCompiler.Features.Compilation.UseCase.Tests.Analysis.Semantics;

[TestFixture]
public class AstIntBinaryOperatorEvaluationTest
{
    [Test]
    public void IntAddOperatorTest()
        => MockUtility.OperatorTestBody<AstAdditionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void IntSubOperatorTest()
        => MockUtility.OperatorTestBody<AstSubtractionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void IntMulOperatorTest()
        => MockUtility.OperatorTestBody<AstMultiplyingExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void IntDivOperatorTest()
        => MockUtility.OperatorTestBody<AstDivisionExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void IntModOperatorTest()
        => MockUtility.OperatorTestBody<AstModuloExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void BitwiseOrOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseOrExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void BitwiseAndOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseAndExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );

    [Test]
    public void BitwiseXorOperatorTest()
        => MockUtility.OperatorTestBody<AstBitwiseXorExpressionNode>( ( visitor, node ) => visitor.Visit( node ), 0, DataTypeFlag.TypeInt );
}
