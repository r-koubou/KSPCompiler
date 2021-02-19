using System.IO;
using System.Text;

namespace KSPCompiler.Domain.Path.Value
{
    public interface IPath
    {
        public string Path { get; }
        public bool Exists { get; }
        public bool IsFile => false;
        public bool IsDirectory => false;

        #region Alias of Stream
        public Stream AsStream( FileMode fileMode )
        {
            return new FileStream( Path, fileMode );
        }
        public StreamWriter AsWriter( FileMode fileMode )
        {
            return new StreamWriter( AsStream( fileMode ) );
        }

        public StreamReader AsReader( )
        {
            return AsReader( Encoding.UTF8 );
        }

        public StreamReader AsReader( Encoding encoding )
        {
            return new StreamReader( AsStream( FileMode.Open ), encoding );
        }
        #endregion

    }
}