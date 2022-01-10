using RkHelper.Text;

using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof( string ), Option = ValueOption.Implicit )]
    public partial class PlainText : IText
    {
        public static readonly IText Empty = new PlainText();

        private PlainText()
        {
            Value = string.Empty;
        }

        private static partial string Validate( string value )
            => StringHelper.IsEmpty( value ) ? string.Empty : value;
    }
}