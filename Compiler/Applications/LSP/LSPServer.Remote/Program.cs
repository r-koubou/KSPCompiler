using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using KSPCompiler.Applications.LSPServer.Core;

using Microsoft.Extensions.Logging;

namespace KSPCompiler.Applications.LSPServer.Remote;

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

            _ = Task.Run( async () => await HandleClientAsync( client ) );
        }
    }

    private static async Task HandleClientAsync( TcpClient client )
    {
        try
        {
            var option = new Server.Option(
                client.GetStream(),
                client.GetStream(),
                LogLevel.Trace
            );

            var server = await Server.CreateAsync( option );
            await server.WaitForExit;
        }
        catch( Exception e)
        {
            Console.WriteLine( e );
        }
        finally
        {
            client.Close();
            Console.WriteLine( "Language Client disconnected." );
        }
    }
}
