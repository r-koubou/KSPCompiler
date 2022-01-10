using RkHelper.Number;

using ValueObjectGenerator;

namespace KSPCompiler.Commons.Text
{
    [ValueObject( typeof(int), Option = ValueOption.Implicit)]
    public partial class Column
    {
        public static readonly Column Unknown = new Column();

        private Column()
        {
            Value = -1;
        }

        private static partial int Validate( int value )
        {
            NumberHelper.ValidateRange( value, 0, int.MaxValue );
            return value;
        }
    }
}