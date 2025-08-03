using System;

namespace KSPCompiler.Shared.EventEmitting;

public readonly record struct TextMessageEvent(
    string Message,
    Exception? Error = null
) : IEvent<string>;
