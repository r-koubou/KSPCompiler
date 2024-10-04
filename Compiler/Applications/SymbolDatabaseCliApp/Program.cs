using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Commands;
using KSPCompiler.Apps.SymbolDbManager.Services;

using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IVariableSymbolDatabaseService, VariableSymbolDatabaseService>();

await using var serviceProvider = serviceCollection.BuildServiceProvider();

ConsoleApp.ServiceProvider = serviceProvider;

var app = ConsoleApp.Create();
app.Add<ImportCommand>();
app.Add<ExportCommand>();
app.Add<DeleteCommand>();

await app.RunAsync( args );
