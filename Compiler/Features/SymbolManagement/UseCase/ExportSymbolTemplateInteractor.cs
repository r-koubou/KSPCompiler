using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.SymbolManagement.UseCase.Abstractions;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

namespace KSPCompiler.Features.SymbolManagement.UseCase;

public class ExportSymbolTemplateInteractor<TSymbol>
    : IExportSymbolTemplateUseCase<TSymbol> where TSymbol : SymbolBase
{
    public async Task<Result<Unit, SymbolManagementFailureReason>> ExecuteAsync( ExportSymbolTemplateInput<TSymbol> input, CancellationToken cancellationToken = default )
    {
        try
        {
            await input.Input.ExportTemplateAsync( cancellationToken );

            return Result<Unit, SymbolManagementFailureReason>.Success( Unit.Default );
        }
        catch( Exception e )
        {
            return Result<Unit, SymbolManagementFailureReason>.Failure( SymbolManagementFailureReason.Other, e );
        }
    }
}
