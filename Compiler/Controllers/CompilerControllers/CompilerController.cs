using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways;
using KSPCompiler.Interactors.Analysis;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Controllers.Compiler;

public record CompilerOption(
    ISyntaxParser SyntaxParser,
    AggregateSymbolTable SymbolTable,
    bool SyntaxCheckOnly,
    bool EnableObfuscation );

public record CompilerResult(
    bool Result,
    Exception? Error,
    string ObfuscatedScript );

public sealed class CompilerController
{
    public CompilerResult Execute( IEventEmitter eventEmitter, CompilerOption option )
        => ExecuteAsync( eventEmitter, option, CancellationToken.None ).GetAwaiter().GetResult();

    // ReSharper disable once UnusedMethodReturnValue.Global
    public async Task<CompilerResult> ExecuteAsync( IEventEmitter eventEmitter, CompilerOption option, CancellationToken cancellationToken )
    {
        // TODO: CompilerMessageManger compilerMessageManger is obsolete. Use IEventEmitter eventEmitter instead.

        try
        {
            var symbolTable = option.SymbolTable;

            var syntaxAnalysisOutput = await ExecuteSyntaxAnalysisAsync( option.SyntaxParser, cancellationToken );
            var ast = syntaxAnalysisOutput.OutputData;

            if( !syntaxAnalysisOutput.Result )
            {
                return new CompilerResult( false, null, string.Empty );
            }

            if( option.SyntaxCheckOnly )
            {
                return new CompilerResult( true, null, string.Empty );
            }

            var preprocessOutput = await ExecutePreprocessAsync( eventEmitter, ast, symbolTable, cancellationToken );

            if( !preprocessOutput.Result )
            {
                return new CompilerResult( false, null, string.Empty );
            }

            var semanticAnalysisOutput = await ExecuteSemanticAnalysisAsync( eventEmitter, ast, symbolTable, cancellationToken );

            if( !semanticAnalysisOutput.Result )
            {
                return new CompilerResult( false, semanticAnalysisOutput.Error, string.Empty );
            }


            if( !option.EnableObfuscation )
            {
                return new CompilerResult( true, null, string.Empty );
            }

            var obfuscationOutput = await ExecuteObfuscationAsync(
                eventEmitter,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable,
                cancellationToken
            );

            Console.WriteLine( obfuscationOutput.OutputData );

            return new CompilerResult(
                obfuscationOutput.Result,
                obfuscationOutput.Error,
                obfuscationOutput.OutputData
            );
        }
        catch( Exception e )
        {
            return new CompilerResult( false, e, string.Empty );
        }
    }

    private async Task<SyntaxAnalysisOutputData> ExecuteSyntaxAnalysisAsync( ISyntaxParser parser, CancellationToken cancellationToken)
    {
        var analyzer = new SyntaxAnalysisInteractor();
        var input = new SyntaxAnalysisInputData( parser );

        return await analyzer.ExecuteAsync( input, cancellationToken );
    }

    private async Task<UnitOutputPort> ExecutePreprocessAsync(
        IEventEmitter compilerMessageManger,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        IPreprocessUseCase preprocessor = new PreprocessInteractor();
        var preprocessInput = new PreprocessInputData(
            new PreprocessInputDataDetail( compilerMessageManger, ast, symbolTable )
        );

        return await preprocessor.ExecuteAsync( preprocessInput, cancellationToken );
    }

    private async Task<SemanticAnalysisOutputData> ExecuteSemanticAnalysisAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var semanticAnalyzer = new SemanticAnalysisInteractor();
        var preprocessInput = new SemanticAnalysisInputData(
            new SemanticAnalysisInputDataDetail( eventEmitter, ast, symbolTable )
        );

        try
        {
            return await semanticAnalyzer.ExecuteAsync( preprocessInput, cancellationToken );
        }
        catch( Exception e )
        {
            return new SemanticAnalysisOutputData(
                false,
                e,
                new SemanticAnalysisOutputDataDetail(
                    ast,
                    symbolTable
                )
            );
        }
    }

    private async Task<ObfuscationOutputData> ExecuteObfuscationAsync(
        IEventEmitter eventEmitter,
        AstCompilationUnitNode ast,
        AggregateSymbolTable symbolTable,
        CancellationToken cancellationToken )
    {
        var obfuscator = new ObfuscationInteractor();
        var input = new ObfuscationInputData(
            new ObfuscationInputDataDetail( eventEmitter, ast, symbolTable )
        );

        try
        {
            return await obfuscator.ExecuteAsync( input, cancellationToken );
        }
        catch( Exception e )
        {
            return new ObfuscationOutputData( false, e, string.Empty );
        }
    }
}
