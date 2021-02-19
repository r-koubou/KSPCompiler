using System.IO;

using ValueObjectGenerator;

namespace KSPCompiler.Domain.Path.Value
{
    [ValueObject( typeof(string), ValueName = "Path" )]
    [NotEmpty]
    public partial class FilePath : IPath
    {
        public bool Exists => File.Exists( Path );
        public bool IsFile => true;
    }
}