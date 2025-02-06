using System;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core;

using Microsoft.Extensions.Logging;

namespace KSPCompiler.Apps.LSPServer.Embedded;

public static class Program
{
    public static async Task Main( string[] _ )
    {
        var option = new Server.Option(
            input: Console.OpenStandardInput(),
            output: Console.OpenStandardOutput(),
            minimumLevel: LogLevel.Trace
        );

        var server = await Server.CreateAsync( option );

        await server.WaitForExit;
    }
}
