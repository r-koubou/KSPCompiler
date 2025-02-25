using System;

namespace KSPCompiler.Interactors.LanguageServer.Ast;

[Flags]
public enum AppearanceFinderMode
{
    // ReSharper disable once UnusedMember.Global
    None = 0x00,
    Declaration = 0x01,
    Reference = 0x02,
    All = Declaration | Reference
}
