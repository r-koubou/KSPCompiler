using System.Text.RegularExpressions;

namespace KSPCompiler.Features.Compilation.Domain.Symbols
{
    /// <summary>
    /// Definitions of regular expressions used in AST analysis in general.
    /// </summary>
    public static class KspRegExpConstants
    {
        /// <summary>
        /// A regular expression that determines whether or not the first character of a variable name contains a numeric character.
        /// KSP is acceptable, but many popular languages are not.
        /// </summary>
        public static readonly Regex NumericPrefix = new Regex( "^.[0-9]", RegexOptions.Compiled );

        /// <summary>
        /// Variable name: Regular expression of the symbol with data type symbol.
        /// </summary>
        public static readonly Regex TypePrefix = new Regex( @"^[\$|\%|\@|\!\?|\~]", RegexOptions.Compiled );

        /// <summary>
        /// Regular expressions of symbols such as preprocessor symbols without data type symbols.
        /// </summary>
        public static readonly Regex NonTypePrefix = new Regex( "^[a-z|A-Z|_]", RegexOptions.Compiled );
    }
}
