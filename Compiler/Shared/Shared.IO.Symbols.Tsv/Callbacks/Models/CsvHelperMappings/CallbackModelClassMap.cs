using CsvHelper.Configuration;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Callbacks.Models.CsvHelperMappings;

public sealed class CallbackModelClassMap : ClassMap<CallbackModel>
{
    public CallbackModelClassMap()
    {
        Map( x => x.Id ).Name( nameof( CallbackModel.Id ) );
        Map( x => x.Name ).Name( nameof( CallbackModel.Name ) );
        Map( x => x.BuiltIn ).Name( nameof( CallbackModel.BuiltIn ) );
        Map( x => x.AllowMultipleDeclaration ).Name( nameof( CallbackModel.AllowMultipleDeclaration ) );
        Map( x => x.BuiltIntoVersion ).Name( nameof( CallbackModel.BuiltIntoVersion ) );
        Map( x => x.Description ).Name( nameof( CallbackModel.Description ) );
        Map( x => x.Arguments ).Name( ConstantValue.ArgumentStartName ).TypeConverter<CallbackArgumentConverter>();
    }
}
