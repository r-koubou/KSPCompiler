using ValueObjectGenerator;

namespace KSPCompiler.Domain.TextFile.Value
{
    [ValueObject( typeof(int), Option = ValueOption.NonValidating )]
    public partial struct Column {}
}