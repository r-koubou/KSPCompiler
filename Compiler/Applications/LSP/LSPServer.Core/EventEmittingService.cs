using KSPCompiler.Gateways.EventEmitting;
using KSPCompiler.Gateways.EventEmitting.Extensions;
using KSPCompiler.Infrastructures.EventEmitting.Default;

using Microsoft.Extensions.Logging;

namespace KSPCompiler.Applications.LSPServer.Core;

public class EventEmittingService
{
    public IEventEmitter Emitter { get; } = new EventEmitter();
}

public class LoggingService
{
    public ILogger<LoggingService> Logger { get; }
    public EventEmittingService EventEmittingService { get; }

    public LoggingService( ILogger<LoggingService> logger, EventEmittingService eventEmittingService )
    {
        Logger               = logger;
        EventEmittingService = eventEmittingService;

        EventEmittingService.Emitter.Subscribe<LogDebugEvent>( evt => Logger.LogDebug( evt.Message ) );
        EventEmittingService.Emitter.Subscribe<LogInfoEvent>( evt => Logger.LogInformation( evt.Message ) );
        EventEmittingService.Emitter.Subscribe<LogWarningEvent>( evt => Logger.LogWarning( evt.Message ) );
        EventEmittingService.Emitter.Subscribe<LogErrorEvent>( evt => Logger.LogError( evt.Message ) );
    }
}
