using System;
using System.Threading;
using System.Threading.Tasks;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.Events;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways.Parsers;
using KSPCompiler.Interactors.Analysis;
using KSPCompiler.UseCases;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Controllers.Compiler;

public record CompilerOption(
    ISyntaxParser SyntaxParser,
    AggregateSymbolTable SymbolTable,
    bool EnableObfuscation );

public record CompilerResult(
    bool Result,
    Exception? Error,
    AstCompilationUnitNode? Ast,
    AggregateSymbolTable SymbolTable,
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
            var syntaxAnalysisOutput = await ExecuteSyntaxAnalysisAsync( option.SyntaxParser, cancellationToken );
            var ast = syntaxAnalysisOutput.OutputData;

            var preprocessOutput = await ExecutePreprocessAsync( eventEmitter, ast, option.SymbolTable, cancellationToken );

            if( !preprocessOutput.Result )
            {
                return new CompilerResult( false, preprocessOutput.Error, ast, option.SymbolTable, string.Empty );
            }

            var semanticAnalysisOutput = await ExecuteSemanticAnalysisAsync( eventEmitter, ast, option.SymbolTable, cancellationToken );

            if( !semanticAnalysisOutput.Result )
            {
                return new CompilerResult( false, semanticAnalysisOutput.Error, ast, option.SymbolTable, string.Empty );
            }


            if( !option.EnableObfuscation )
            {
                return new CompilerResult( true, null, ast, option.SymbolTable, string.Empty );
            }

            var obfuscationOutput = await ExecuteObfuscationAsync(
                eventEmitter,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable,
                cancellationToken
            );

            return new CompilerResult(
                obfuscationOutput.Result,
                obfuscationOutput.Error,
                ast,
                option.SymbolTable,
                obfuscationOutput.OutputData
            );
        }
        catch( Exception e )
        {
            return new CompilerResult( false, e, null, option.SymbolTable, string.Empty );
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
