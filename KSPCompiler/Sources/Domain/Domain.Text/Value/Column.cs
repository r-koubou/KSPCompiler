using ValueObjectGenerator;

namespace KSPCompiler.Domain.TextFile.Value
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit | ValueOption.NonValidating )]
    public partial struct Column {}
}