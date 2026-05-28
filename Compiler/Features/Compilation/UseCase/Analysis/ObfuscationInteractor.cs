using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;
using KSPCompiler.Shared;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class ObfuscationInteractor : IObfuscationUseCase
{
    public async Task<Result<ObfuscationOutput, CompilationFailureReason>> ExecuteAsync( ObfuscationInput input, CancellationToken cancellationToken = default )

    {
        var obfuscatedStringBuilder = new StringBuilder( input.DefaultOutputBufferCapacity );

        var messageManger = input.EventEmitter;
        var compilationUnit = input.CompilationUnitNode;
        var symbolTable = input.SymbolTable;

        var context = new ObfuscatorContext( obfuscatedStringBuilder, messageManger, symbolTable );
        var obfuscator = new Obfuscator( context, obfuscatedStringBuilder );

        try
        {
            obfuscator.Traverse( compilationUnit );

            var outputData = new ObfuscationOutput( obfuscatedStringBuilder.ToString() );

            await Task.CompletedTask;
            return Result<ObfuscationOutput, CompilationFailureReason>.Success( outputData );
        }
        catch( ArgumentException e )
        {
            return Result<ObfuscationOutput, CompilationFailureReason>.Failure( CompilationFailureReason.SemanticsError, e );
        }
        catch( KeyNotFoundException e )
        {
            return Result<ObfuscationOutput, CompilationFailureReason>.Failure( CompilationFailureReason.SymbolNotFound, e );
        }
        catch( Exception e )
        {
            return Result<ObfuscationOutput, CompilationFailureReason>.Failure( CompilationFailureReason.Other, e );
        }
    }
}
