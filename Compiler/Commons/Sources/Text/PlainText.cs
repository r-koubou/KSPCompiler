using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof( string ), Option = ValueOption.NonValidating )]
    public partial class PlainText : IText
    {
        public static IText Empty { get; } = new PlainText( "" );
    }
}