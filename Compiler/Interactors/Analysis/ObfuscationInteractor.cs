using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Interactors.Analysis.Obfuscators;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Interactors.Analysis;

public class ObfuscationInteractor : IObfuscationUseCase
{
    public Task<ObfuscationOutputData> ExecuteAsync( ObfuscationInputData parameter, CancellationToken cancellationToken = default )
    {
        var output = new StringBuilder( parameter.InputData.DefaultOutputBufferCapacity );

        var messageManger = parameter.InputData.MessageManager;
        var compilationUnit = parameter.InputData.CompilationUnitNode;
        var symbolTable = parameter.InputData.SymbolTable;

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
            => new( result, error, output.ToString() );
    }

    public ObfuscationOutputData Execute( ObfuscationInputData input )
        => ExecuteAsync( input ).GetAwaiter().GetResult();
}
