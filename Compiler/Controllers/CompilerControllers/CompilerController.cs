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
    AggregateSymbolTable symbolTable,
    bool SyntaxCheckOnly,
    bool EnableObfuscation );

public sealed class CompilerController
{
    public void Execute( ICompilerMessageManger compilerMessageManger, CompilerOption option )
    {
        try
        {
            var symbolTable = option.symbolTable;

            var ast = option.SyntaxParser.Parse();

            if( option.SyntaxCheckOnly )
            {
                return;
            }

            var preprocessOutput = ExecutePreprocess( compilerMessageManger, ast, symbolTable );

            if( !preprocessOutput.Result )
            {
                Console.Error.WriteLine( preprocessOutput.Error );
                return;
            }

            var semanticAnalysisOutput = ExecuteSemanticAnalysis( compilerMessageManger, ast, symbolTable );

            if( !option.EnableObfuscation )
            {
                return;
            }

            // TODO Obfuscation
            var obfuscationOOutput = ExecuteObfuscation(
                compilerMessageManger,
                semanticAnalysisOutput.OutputData.CompilationUnitNode,
                semanticAnalysisOutput.OutputData.SymbolTable
            );

        }
        catch( Exception e )
        {
            // ignored
            Console.Error.WriteLine( e );
        }
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
