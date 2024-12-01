using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways;
using KSPCompiler.Interactors.Analysis;
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
    public CompilerResult Execute( ICompilerMessageManger compilerMessageManger, CompilerOption option )
    {
        try
        {
            var symbolTable = option.SymbolTable;

            var syntaxAnalysisOutput = ExecuteSyntaxAnalysis( option.SyntaxParser );
            var ast = syntaxAnalysisOutput.OutputData;

            if( !syntaxAnalysisOutput.Result )
            {
                return new CompilerResult( false, null, string.Empty );
            }

            if( option.SyntaxCheckOnly )
            {
                return new CompilerResult( true, null, string.Empty );
            }

            var preprocessOutput = ExecutePreprocess( compilerMessageManger, ast, symbolTable );

            if( !preprocessOutput.Result )
            {
                return new CompilerResult( false, null, string.Empty );
            }

            var semanticAnalysisOutput = ExecuteSemanticAnalysis( compilerMessageManger, ast, symbolTable );

            if( !semanticAnalysisOutput.Result )
            {
                return new CompilerResult( false, semanticAnalysisOutput.Error, string.Empty );
            }


            if( !option.EnableObfuscation )
            {
                return new CompilerResult( true, null, string.Empty );
            }

            var obfuscationOutput = ExecuteObfuscation(
                compilerMessageManger,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable
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

    private SyntaxAnalysisOutputData ExecuteSyntaxAnalysis( ISyntaxParser parser )
    {
        var analyzer = new SyntaxAnalysisInteractor();
        var input = new SyntaxAnalysisInputData( parser );

        return analyzer.Execute( input );
    }

    private PreprocessOutputData ExecutePreprocess( ICompilerMessageManger compilerMessageManger, AstCompilationUnitNode ast, AggregateSymbolTable symbolTable )
    {
        var preprocessor = new PreprocessInteractor();
        var preprocessInput = new PreprocessInputData(
            new PreprocessInputDataDetail( compilerMessageManger, ast, symbolTable )
        );

        try
        {
            return preprocessor.Execute( preprocessInput );
        }
        catch( Exception e )
        {
            return new PreprocessOutputData(
                false,
                e,
                new PreprocessOutputDataDetail(
                    compilerMessageManger,
                    ast,
                    symbolTable
                )
            );
        }
    }

    private SemanticAnalysisOutputData ExecuteSemanticAnalysis( ICompilerMessageManger compilerMessageManger, AstCompilationUnitNode ast, AggregateSymbolTable symbolTable )
    {
        var semanticAnalyzer = new SemanticAnalysisInteractor();
        var preprocessInput = new SemanticAnalysisInputData(
            new SemanticAnalysisInputDataDetail( compilerMessageManger, ast, symbolTable )
        );

        try
        {
            return semanticAnalyzer.Execute( preprocessInput );
        }
        catch( Exception e )
        {
            return new SemanticAnalysisOutputData(
                false,
                e,
                new SemanticAnalysisOutputDataDetail(
                    compilerMessageManger,
                    ast,
                    symbolTable
                )
            );
        }
    }

    private ObfuscationOutputData ExecuteObfuscation( ICompilerMessageManger compilerMessageManger, AstCompilationUnitNode ast, AggregateSymbolTable symbolTable )
    {
        var obfuscator = new ObfuscationInteractor();
        var input = new ObfuscationInputData(
            new ObfuscationInputDataDetail( compilerMessageManger, ast, symbolTable )
        );

        try
        {
            return obfuscator.Execute( input );
        }
        catch( Exception e )
        {
            return new ObfuscationOutputData( false, e, string.Empty );
        }
    }
}
