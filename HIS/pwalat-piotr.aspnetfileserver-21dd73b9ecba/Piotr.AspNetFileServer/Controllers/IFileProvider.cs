using System.IO;

namespace Piotr.AspNetFileServer.Controllers
{
    public interface IFileProvider
    {
        bool Exists(string name);
        FileStream Open(string name);
        long GetLength(string name);
        string GetFilePath(string name);
    }
}