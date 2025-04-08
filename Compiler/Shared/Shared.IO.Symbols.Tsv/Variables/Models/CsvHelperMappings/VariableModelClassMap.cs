using CsvHelper.Configuration;

namespace KSPCompiler.Shared.IO.Symbols.Tsv.Variables.Models.CsvHelperMappings;

public sealed class VariableModelClassMap : ClassMap<VariableModel>
{
    public VariableModelClassMap()
    {
        Map( x => x.Id ).Name( nameof( VariableModel.Id ) );
        Map( x => x.Name ).Name( nameof( VariableModel.Name ) );
        Map( x => x.BuiltIn ).Name( nameof( VariableModel.BuiltIn ) );
        Map( x => x.Description ).Name( nameof( VariableModel.Description ) );
        Map( x => x.BuiltIntoVersion ).Name( nameof( VariableModel.BuiltIntoVersion ) );
    }
}
