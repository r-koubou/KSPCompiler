using System;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Omnisharp;

using Microsoft.Extensions.Logging;

namespace KSPCompiler.Applications.LSPServer.Embedded;

public static class Program
{
    public static async Task Main( string[] _ )
    {
        var option = new Server.Option(
            input: Console.OpenStandardInput(),
            output: Console.OpenStandardOutput(),
            loggerFactory: ZLoggerFactory.Create(),
            minimumLevel: LogLevel.Trace
        );

        var server = await Server.CreateAsync( option );

        await server.WaitForExit;
    }
}
