using System.IO;

namespace KSPCompiler.Shared.Path
{
    public class FilePath : IPath
    {
        public string Path { get; }
        public bool Exists => File.Exists( Path );
        public bool IsFile => true;
        public bool IsDirectory => false;

        public FilePath( string path )
        {
            Path = path;
        }

        public void CreateNew()
        {
            if( !Exists )
            {
                using var _ = File.Create( this.Path );
            }
        }

        public void CreateDirectory()
        {
            var dir = System.IO.Path.GetDirectoryName( Path );

            if( dir == null )
            {
                return;
            }

            if( Directory.Exists( dir ) )
            {
                return;
            }

            Directory.CreateDirectory( dir );
        }

        public Stream OpenStream( FileMode mode, FileAccess access )
        {
            CreateDirectory();
            return File.Open( Path, mode, access );
        }

        public Stream OpenReadStream()
        {
            return File.OpenRead( Path );
        }

        public Stream OpenWriteStream()
        {
            CreateDirectory();
            return File.OpenWrite( Path );
        }

        public override string ToString() => Path;

        public static implicit operator FilePath( string path ) => new( path );
    }
}
