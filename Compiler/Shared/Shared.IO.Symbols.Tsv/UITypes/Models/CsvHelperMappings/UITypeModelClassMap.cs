using CsvHelper.Configuration;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.UITypes.Models.CsvHelperMappings;

public sealed class UITypeModelClassMap : ClassMap<UITypeModel>
{
    public UITypeModelClassMap()
    {
        Map( x => x.Name ).Name( nameof( UITypeModel.Name ) );
        Map( x => x.BuiltIn ).Name( nameof( UITypeModel.BuiltIn ) );
        Map( x => x.DataType ).Name( nameof( UITypeModel.DataType ) );
        Map( x => x.Description ).Name( nameof( UITypeModel.Description ) );
        Map( x => x.BuiltIntoVersion ).Name( nameof( UITypeModel.BuiltIntoVersion ) );
        Map( x => x.InitializerRequired ).Name( nameof( UITypeModel.InitializerRequired ) );
        Map( x => x.Arguments ).Name( ConstantValue.ArgumentStartName ).TypeConverter<UITypeArgumentConverter>();
    }
}
