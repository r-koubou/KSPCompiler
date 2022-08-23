using RkHelper.Text;

using ValueObjectGenerator;

namespace KSPCompiler.Domain.CompilerMessages;

[ValueObject(typeof(string), Option = ValueOption.Implicit)]
public partial class CompilerMessageText
{
    private static partial string Validate( string value )
        => StringHelper.IsEmpty( value ) ? string.Empty : value;
}