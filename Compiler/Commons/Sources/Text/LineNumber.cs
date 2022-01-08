using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit | ValueOption.NonValidating )]
    public partial struct LineNumber
    {
        public static readonly LineNumber Unknown = -1;
    }
}