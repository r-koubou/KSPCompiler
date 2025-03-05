using CsvHelper.Configuration;

namespace KSPCompiler.Features.SymbolManagement.Infrastructures.ExternalSymbol.Tsv.Variables.Models.CsvHelperMappings;

public sealed class VariableModelClassMap : ClassMap<VariableModel>
{
    public VariableModelClassMap()
    {
        Map( x => x.Name ).Name( nameof( VariableModel.Name ) );
        Map( x => x.BuiltIn ).Name( nameof( VariableModel.BuiltIn ) );
        Map( x => x.Description ).Name( nameof( VariableModel.Description ) );
        Map( x => x.BuiltIntoVersion ).Name( nameof( VariableModel.BuiltIntoVersion ) );
    }
}
