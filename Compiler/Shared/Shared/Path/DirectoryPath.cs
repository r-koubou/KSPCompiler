using System.IO;

namespace KSPCompiler.Shared.Path
{
    public class DirectoryPath : IPath
    {
        public string Path { get; }
        public bool Exists => Directory.Exists( Path );
        public bool IsFile => false;
        public bool IsDirectory => true;

        public DirectoryPath( string path )
        {
            Path = path;
        }

        public void CreateNew()
        {
            if( !Exists )
            {
                Directory.CreateDirectory( Path );
            }
        }

        public override string ToString() => Path;

        public static implicit operator DirectoryPath( string path ) => new( path );
    }
}
