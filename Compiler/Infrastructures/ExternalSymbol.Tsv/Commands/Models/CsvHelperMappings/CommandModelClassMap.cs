using CsvHelper.Configuration;

namespace KSPCompiler.ExternalSymbol.Tsv.Commands.Models.CsvHelperMappings;

public sealed class CommandModelClassMap : ClassMap<CommandModel>
{
    public CommandModelClassMap()
    {
        Map( x => x.Name ).Name( nameof( CommandModel.Name ) );
        Map( x => x.BuiltIn ).Name( nameof( CommandModel.BuiltIn ) );
        Map( x => x.BuiltIntoVersion ).Name( nameof( CommandModel.BuiltIntoVersion ) );
        Map( x => x.Description ).Name( nameof( CommandModel.Description ) );
        Map( x => x.ReturnType ).Name( nameof( CommandModel.ReturnType ) );
        Map( x => x.Arguments ).Name( ConstantValue.ArgumentStartName ).TypeConverter<CommandArgumentConverter>();
    }
}
