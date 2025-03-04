using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Features.Compilation.UseCase.Analysis.Abstractions;
using KSPCompiler.Features.Compilation.UseCase.Analysis.Obfuscators;

namespace KSPCompiler.Features.Compilation.UseCase.Analysis;

public class ObfuscationInteractor : IObfuscationUseCase
{
    public Task<ObfuscationOutputData> ExecuteAsync( ObfuscationInputData parameter, CancellationToken cancellationToken = default )
    {
        var output = new StringBuilder( parameter.Input.DefaultOutputBufferCapacity );

        var messageManger = parameter.Input.EventEmitter;
        var compilationUnit = parameter.Input.CompilationUnitNode;
        var symbolTable = parameter.Input.SymbolTable;

        var context = new ObfuscatorContext( output, messageManger, symbolTable );
        var obfuscator = new Obfuscator( context, output );

        try
        {
            obfuscator.Traverse( compilationUnit );
        }
        catch( Exception e )
        {
            return Task.FromResult( CreateOutputData( false, e ) );
        }

        return Task.FromResult( CreateOutputData( true, null ) );

        ObfuscationOutputData CreateOutputData( bool result, Exception? error )
            => new( output.ToString(), result, error );
    }

    public ObfuscationOutputData Execute( ObfuscationInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();
}
