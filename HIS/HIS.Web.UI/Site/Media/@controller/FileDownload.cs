using HIS.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;

namespace HIS.Web.UI 
{
    public class FileDownloadController : ApiController
    {
        private const string MapNamePrefix = "FileServerMap";
        private ApplicationPath appPath;

        public IFileProvider FileProvider { get; set; }

        public FileDownloadController(ApplicationPath appPath)
        {
            this.appPath = appPath;
            FileProvider = new FileProvider();
        }

        private ContentInfo GetContentInfoFromRequest(HttpRequestMessage request, long entityLength)
        {
            var result = new ContentInfo
            {
                From = 0,
                To = entityLength,
                IsPartial = false,
                Length = entityLength
            };
            RangeHeaderValue rangeHeader = request.Headers.Range;
            if (rangeHeader != null && rangeHeader.Ranges.Count != 0)
            {
                //we support only one range
                if (rangeHeader.Ranges.Count > 1)
                {
                    //we probably return other status code here
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }
                RangeItemHeaderValue range = rangeHeader.Ranges.First();
                if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > entityLength)
                {
                    throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                }

                result.From = range.From ?? 0;
                result.To = range.To ?? entityLength;
                result.IsPartial = true;
                result.Length = entityLength;
                if (range.From.HasValue && range.To.HasValue)
                {
                    result.Length = range.To.Value - range.From.Value;
                }
                else if (range.From.HasValue)
                {
                    result.Length = entityLength - range.From.Value;
                }
                else if (range.To.HasValue)
                {
                    result.Length = range.To.Value;
                }
            }

            return result;
        }

        private void SetResponseHeaders(HttpResponseMessage response, ContentInfo contentInfo,
            long fileLength, string fileName, byte[] hash)
        {
            response.Headers.AcceptRanges.Add("bytes");
            response.StatusCode = contentInfo.IsPartial ? HttpStatusCode.PartialContent
                : HttpStatusCode.OK;
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentLength = contentInfo.Length;

            if (hash.Length > 0)
                response.Content.Headers.ContentMD5 = hash;

            if (contentInfo.IsPartial)
            {
                response.Content.Headers.ContentRange
                    = new ContentRangeHeaderValue(contentInfo.From, contentInfo.To, fileLength);
            }
        }


        private string GenerateMapNameFromName(string fileName)
        {
            return String.Format("{0}_{1}", MapNamePrefix, fileName);
        }

        [HttpGet]
        public HttpResponseMessage Head(string fileName,string resourcetype)
        {
            string filePath = GetFilePath(fileName, resourcetype);

            if (String.IsNullOrEmpty(filePath))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //string fileName = GetFileName(name);
            if (!Exists(filePath))
            {
                //if file does not exist return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            long fileLength = GetLength(filePath);
            ContentInfo contentInfo = GetContentInfoFromRequest(this.Request, fileLength);

            var response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(new byte[10]);
            SetResponseHeaders(response, contentInfo, fileLength, fileName, new byte[0]);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Get(string fileName,string resourcetype)
        {
            string filePath = GetFilePath(fileName, resourcetype);

            if (!Exists(filePath))
            {   //if file does not exist return 404
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            long fileLength = GetLength(filePath);
            ContentInfo contentInfo = GetContentInfoFromRequest(this.Request, fileLength);
            string mapName = GenerateMapNameFromName(fileName);

            MemoryMappedFile mmf = null;

            try
            {
                mmf = MemoryMappedFile.OpenExisting(mapName, MemoryMappedFileRights.Read);
            }
            catch (FileNotFoundException)
            {
                //every time we use an exception to control flow a kitten dies

                mmf = MemoryMappedFile.CreateFromFile(FileProvider.Open(filePath), mapName, fileLength,
                                                      MemoryMappedFileAccess.Read, null, HandleInheritability.None,
                                                      false);
            }
            using (mmf)
            {
                Stream stream
                    = contentInfo.IsPartial
                    ? mmf.CreateViewStream(contentInfo.From, contentInfo.Length, MemoryMappedFileAccess.Read)
                    : mmf.CreateViewStream(0, fileLength, MemoryMappedFileAccess.Read);  


                var response = new HttpResponseMessage();
                response.Content = new StreamContent(stream);

                byte[] md5hash = GetCrc(stream); 

                SetResponseHeaders(response, contentInfo, fileLength, fileName, md5hash);

                return response;
            }
        }

        private byte[] GetMd5Hash(Stream stream)
        {

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = new byte[4096];
                int byteCount = 0;
                while ((byteCount = stream.Read(data, 0, data.Length)) > 0)
                {
                    md5.TransformBlock(data, 0, byteCount, null, 0); // feed the data to MD5 algorithm

                    // do something useful with the actual read data here
                }
                md5.TransformFinalBlock(data, 0, 0); // tell the algorithm that all data is read
                stream.Seek(0, SeekOrigin.Begin);
                byte[] hash = md5.Hash;

                return hash;
            }
        }

        private byte[] GetCrc(Stream stream)
        {

            using (CrcStream cStream = new CrcStream(stream))
            {

                byte[] buffer = new byte[4096];
                int length;

                while ((length = cStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Do whatever you need to do in here
                }

                stream.Seek(0, SeekOrigin.Begin);

                return BitConverter.GetBytes(cStream.ReadCrc);
            }

        }

        private string GetFilePath(string filename, string resourceType)
        {
            switch (resourceType)
            { 
                case Constants.Image:
                    return Path.Combine(appPath.Images, filename);
                case Constants.Music:
                    return Path.Combine(appPath.Musics, filename);                     
                case Constants.Video:
                    return Path.Combine(appPath.Videos, filename);
                   
            }

            return String.Empty;
        }

        public bool Exists(string name)
        {
            //make sure we dont access directories outside of our store for security reasons
            return File.Exists(name);
        }

        public long GetLength(string name)
        {
            return new FileInfo(name).Length;
        }
    }
}