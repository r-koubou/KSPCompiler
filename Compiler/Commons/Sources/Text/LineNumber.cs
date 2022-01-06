using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit | ValueOption.NonValidating)]
    [NotNegative]
    public partial struct LineNumber {}
}