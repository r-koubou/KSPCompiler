namespace KSPCompiler.Domain.Symbols;

public class DataTypeInformation
{
    public DataTypeFlag DataTypeFlag { get; set; }
    public DataTypeAttributeFlag DataTypeAttribute { get; set; }

    public DataTypeInformation(
        DataTypeFlag dataTypeFlag = DataTypeFlag.None,
        DataTypeAttributeFlag dataTypeAttribute = DataTypeAttributeFlag.None )
    {
        DataTypeFlag      = dataTypeFlag;
        DataTypeAttribute = dataTypeAttribute;
    }
}
