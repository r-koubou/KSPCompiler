using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Symbols;
using KSPCompiler.UseCases.Symbols;

namespace KSPCompiler.Interactors.Symbols;

public sealed class LoadBuiltinSymbolInteractor : ILoadBuiltinSymbolUseCase
{
    public async Task<LoadBuiltinSymbolOutputData> ExecuteAsync( LoadBuiltinSymbolInputData parameter, CancellationToken cancellationToken = default )
    {
        var symbolTables = AggregateSymbolTable.Default();

        try
        {
            symbolTables.BuiltInVariables.AddRange( await parameter.InputData.VariableSymbolRepository.FindAllAsync( cancellationToken ) );
            symbolTables.UITypes.AddRange( await parameter.InputData.UITypeSymbolRepository.FindAllAsync( cancellationToken ) );
            symbolTables.Commands.AddRange( await parameter.InputData.CommandSymbolRepository.FindAllAsync( cancellationToken ) );

            var foundCallbacks = await parameter.InputData.CallbackSymbolRepository.FindAllAsync( cancellationToken );

            foreach( var x in foundCallbacks )
            {
                if( x.AllowMultipleDeclaration )
                {
                    symbolTables.BuiltInCallbacks.AddAsOverload( x, x.Arguments );
                }
                else
                {
                    symbolTables.BuiltInCallbacks.AddAsNoOverload( x );
                }
            }

            return new LoadBuiltinSymbolOutputData( symbolTables, true );
        }
        catch( Exception e )
        {
            return new LoadBuiltinSymbolOutputData( symbolTables, false, e );
        }
    }
}
