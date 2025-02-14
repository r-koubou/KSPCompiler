using Microsoft.Extensions.Logging;

using ZLogger;
using ZLogger.Formatters;

namespace KSPCompiler.Applications.LSPServer.Embedded;

public static class ZLoggerFactory
{
    public static ILoggerFactory Create()
    {
        return LoggerFactory.Create( configure =>
            {
                configure.SetMinimumLevel( LogLevel.Trace );
                configure.AddZLoggerConsole( options =>
                    {
                        options.UsePlainTextFormatter( SetupFormatter );

                    }
                );
                configure.AddZLoggerFile( "log.txt", options =>
                    {
                        options.UsePlainTextFormatter( SetupFormatter );
                    }
                );
            }
        );
    }

    private static void SetupFormatter( PlainTextZLoggerFormatter formatter )
    {
        formatter.SetPrefixFormatter(
            $"{0:utc-longdate} [{1:short}]",
            ( in MessageTemplate template, in LogInfo info )
                => template.Format( info.Timestamp, info.LogLevel
                )
        );
    }
}
