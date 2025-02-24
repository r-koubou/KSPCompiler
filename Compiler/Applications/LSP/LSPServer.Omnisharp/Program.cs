using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAppFramework;

using KSPCompiler.Applications.LSPServer.Omnisharp;

using Microsoft.Extensions.Logging;

var app = ConsoleApp.Create();

app.Add( "stdio", ServerApplication.RunStdIoAsync );
app.Add( "socket", ServerApplication.RunSocketAsync );

await app.RunAsync( args );

static class ServerApplication
{
    #region Stdio mode
    /// <summary>
    /// Run server in stdio mode
    /// </summary>
    /// <param name="cancellationToken"></param>
    public static async Task RunStdIoAsync( CancellationToken cancellationToken )
    {
        await Console.Error.WriteLineAsync( "Server started in stdio mode" );

        var option = new Server.Option(
            input: Console.OpenStandardInput(),
            output: Console.OpenStandardOutput(),
            loggerFactory: ZLoggerFactory.Create(),
            minimumLevel: LogLevel.Trace
        );

        var server = await Server.CreateAsync( option );

        await server.WaitForExit;
    }
    #endregion

    #region Socket mode
    /// <summary>
    /// Run server in socket mode
    /// </summary>
    /// <param name="port">-p, Port number</param>
    /// <param name="cancellationToken"></param>
    public static async Task RunSocketAsync( int port, CancellationToken cancellationToken )
    {
        await Console.Out.WriteLineAsync( $"Server started in socket mode on port {port}" );

        var listener = new TcpListener( IPAddress.Loopback, port );

        listener.Start();

        while( true )
        {
            var client = await listener.AcceptTcpClientAsync( cancellationToken );
            await Console.Out.WriteLineAsync( "Language Client connected." );

            _ = Task.Run( async () => await HandleSocketClientAsync( client ), cancellationToken );
        }

        // ReSharper disable once FunctionNeverReturns
    }

    private static async Task HandleSocketClientAsync( TcpClient client )
    {
        try
        {
            var option = new Server.Option(
                input: client.GetStream(),
                output: client.GetStream(),
                loggerFactory: ZLoggerFactory.Create(),
                minimumLevel: LogLevel.Trace
            );

            var server = await Server.CreateAsync( option );
            await server.WaitForExit;
        }
        catch( Exception e )
        {
            Console.WriteLine( e );
        }
        finally
        {
            client.Close();
            await Console.Out.WriteLineAsync( "Language Client disconnected." );
        }
    }
    #endregion
}
