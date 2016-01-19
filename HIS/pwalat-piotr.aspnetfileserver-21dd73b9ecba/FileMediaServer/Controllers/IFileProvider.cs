using System.IO;

namespace FileMediaServer.Controllers
{
    public interface IFileProvider
    {
        bool Exists(string name);
        FileStream Open(string name);
        long GetLength(string name);
    }
}