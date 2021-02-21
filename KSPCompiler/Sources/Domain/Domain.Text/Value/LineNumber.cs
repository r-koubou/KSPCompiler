using ValueObjectGenerator;

namespace KSPCompiler.Domain.TextFile.Value
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit | ValueOption.NonValidating)]
    [NotNegative]
    public partial struct LineNumber {}
}