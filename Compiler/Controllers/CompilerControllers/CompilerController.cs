using System;

using KSPCompiler.Domain.Ast.Nodes.Blocks;
using KSPCompiler.Domain.CompilerMessages;
using KSPCompiler.Domain.Symbols;
using KSPCompiler.Gateways;
using KSPCompiler.Interactors.Analysis;
using KSPCompiler.UseCases.Analysis;

namespace KSPCompiler.Controllers.Compiler;

public record CompilerOption( ISyntaxParser SyntaxParser);

public sealed class CompilerController
{
    public void Execute( ICompilerMessageManger compilerMessageManger, CompilerOption option )
    {
        try
        {
            var symbolTable = new AggregateSymbolTable(
                new VariableSymbolTable(),
                new UITypeSymbolTable(),
                new CommandSymbolTable(),
                new CallbackSymbolTable(),
                new CallbackSymbolTable(),
                new UserFunctionSymbolTable(),
                new PreProcessorSymbolTable()
            );

            var ast = option.SyntaxParser.Parse();

            var preprocessOutput = ExecutePreprocess( compilerMessageManger, ast, symbolTable );

            if( !preprocessOutput.Result )
            {
                Console.Error.WriteLine( preprocessOutput.Error );
                return;
            }

            var semanticAnalysisOutput = ExecuteSemanticAnalysis( compilerMessageManger, ast, symbolTable );
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
}
