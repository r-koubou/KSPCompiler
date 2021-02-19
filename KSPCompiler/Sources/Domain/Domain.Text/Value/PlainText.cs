using ValueObjectGenerator;

namespace KSPCompiler.Domain.TextFile.Value
{
    [ValueObject( typeof( string ), Option = ValueOption.NonValidating )]
    public partial class PlainText : IText
    {
        public static IText Empty { get; } = new PlainText( "" );
    }
}