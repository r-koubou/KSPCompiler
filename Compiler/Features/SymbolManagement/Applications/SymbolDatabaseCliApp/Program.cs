using System;

using ConsoleAppFramework;

using KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Commands;
using KSPCompiler.Features.SymbolManagement.Applications.SymbolDbManager.Services;
using KSPCompiler.Shared;
using KSPCompiler.Shared.Domain.Compilation.Symbols;
using KSPCompiler.Shared.EventEmitting;
using KSPCompiler.Shared.EventEmitting.Extensions;

using Microsoft.Extensions.DependencyInjection;

var eventEmitter = new EventEmitter();
var subscriptions = new CompositeDisposable();

eventEmitter.Subscribe<TextMessageEvent>( e =>
    {
        Console.WriteLine( e.Message );
    }
).AddTo( subscriptions );

var serviceCollection = new ServiceCollection();

serviceCollection.AddSingleton<IVariableSymbolDatabaseService>( new VariableSymbolDatabaseService( eventEmitter: eventEmitter ) );
serviceCollection.AddSingleton<ICommandSymbolDatabaseService>( new CommandSymbolDatabaseService( eventEmitter: eventEmitter ) );
serviceCollection.AddSingleton<ICallbackSymbolDatabaseService>( new CallbackSymbolDatabaseService( eventEmitter: eventEmitter ) );
serviceCollection.AddSingleton<IUITypeSymbolDatabaseService>( new UITypeSymbolDatabaseService( eventEmitter: eventEmitter ) );

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
