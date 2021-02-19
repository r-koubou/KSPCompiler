using System.IO;

using ValueObjectGenerator;

namespace KSPCompiler.Domain.Path.Value
{
    [ValueObject( typeof(string), ValueName = "Path" )]
    [NotEmpty]
    public partial class DirectoryPath : IPath
    {
        public bool Exists => Directory.Exists( Path );
        public bool IsDirectory => true;
    }
}