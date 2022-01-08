using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit | ValueOption.NonValidating )]
    public partial struct Column
    {
        public static readonly Column Unknown = -1;
    }
}