using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using KSPCompiler.LSPServer.Core;

using Microsoft.Extensions.Logging;

namespace KSPCompiler.LSPServer.Remote;

class Program
{
    private static IPAddress IpAddress => IPAddress.Loopback;
    private const int Port = 12345;

    static async Task Main( string[] args )
    {
        var listener = new TcpListener( IpAddress, Port );

        listener.Start();
        Console.WriteLine( $"Language Server started on port {Port}." );

        while( true )
        {
            var client = await listener.AcceptTcpClientAsync();
            Console.WriteLine( "Language Client connected." );

            var option = new Server.Option(
                client.GetStream(),
                client.GetStream(),
                LogLevel.Trace
            );

            var server = await Server.Create( option );
            await server.WaitForExit;
        }
    }
}
