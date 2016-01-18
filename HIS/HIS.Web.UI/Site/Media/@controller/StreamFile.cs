using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;



namespace HIS.Web.UI.Site.Media._controller
{
    public class StreamFile
    {
        private readonly string _filename;

        public StreamFile(string filename, string ext)
        {
            if (ext.Equals("mp4"))
                _filename = @"C:\Temp\WorkingFolder\Resources\Video\" + filename;//+ "." + ext;
            else if (ext.Equals("mp3"))
                _filename = @"C:\Temp\WorkingFolder\Resources\Music\" + filename;// +"." + ext;
            else if (ext.Equals("jpg"))
                _filename = @"C:\Temp\WorkingFolder\Resources\Image\" + filename;// +"." + ext;
            else
                _filename = @"C:\Temp\WorkingFolder\Resources\Others\VideoJS" + filename;// +"." + ext;
        }

        public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[65536];

                using (var video = File.Open(_filename, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException ex)
            {
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}
