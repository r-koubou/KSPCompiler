using ConsoleAppFramework;

using KSPCompiler.Apps.SymbolDbManager.Commands;
using KSPCompiler.Apps.SymbolDbManager.Services;

using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IVariableSymbolDatabaseService, VariableSymbolDatabaseService>();
serviceCollection.AddSingleton<ICommandSymbolDatabaseService, CommandSymbolDatabaseService>();
serviceCollection.AddSingleton<ICallbackSymbolDatabaseService, CallbackSymbolDatabaseService>();
serviceCollection.AddSingleton<IUITypeSymbolDatabaseService, UITypeSymbolDatabaseService>();

await using var serviceProvider = serviceCollection.BuildServiceProvider();

ConsoleApp.ServiceProvider = serviceProvider;

var app = ConsoleApp.Create();
app.Add<ImportCommand>();
app.Add<ExportCommand>();
app.Add<DeleteCommand>();

await app.RunAsync( args );
