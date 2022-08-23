using RkHelper.Text;

using ValueObjectGenerator;

namespace KSPCompiler.Domain.Ast.Symbols;

[ValueObject( typeof(string), Option = ValueOption.Implicit )]
public partial class SymbolName
{
    public static readonly SymbolName Empty = new SymbolName();

    private SymbolName()
    {
        Value = string.Empty;
    }

    private static partial string Validate( string value )
        => StringHelper.IsEmpty( value ) ? string.Empty : value;
}