using ConsoleAppFramework;

using KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Commands;
using KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;
using KSPCompiler.Shared.Domain.Compilation.Symbols;

using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IVariableSymbolDatabaseService, VariableSymbolDatabaseService>();
serviceCollection.AddSingleton<ICommandSymbolDatabaseService, CommandSymbolDatabaseService>();
serviceCollection.AddSingleton<ICallbackSymbolDatabaseService, CallbackSymbolDatabaseService>();
serviceCollection.AddSingleton<IUITypeSymbolDatabaseService, UITypeSymbolDatabaseService>();

serviceCollection.AddSingleton<SymbolTemplateService<VariableSymbol>, VariableSymbolTemplateService>();
serviceCollection.AddSingleton<SymbolTemplateService<CommandSymbol>, CommandSymbolTemplateService>();
serviceCollection.AddSingleton<SymbolTemplateService<CallbackSymbol>, CallbackSymbolTemplateService>();
serviceCollection.AddSingleton<SymbolTemplateService<UITypeSymbol>, UITypeSymbolTemplateService>();

await using var serviceProvider = serviceCollection.BuildServiceProvider();

ConsoleApp.ServiceProvider = serviceProvider;

var app = ConsoleApp.Create();
app.Add<ImportCommand>();
app.Add<ExportCommand>();
app.Add<DeleteCommand>();
app.Add<TemplateCommand>();

await app.RunAsync( args );
