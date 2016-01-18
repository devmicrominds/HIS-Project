using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace HIS.Web.UI.Site.Media._controller
{
    public class StreamsFileController : ApiController
    {
        private string _filename;
        private string _ext;
        // GET: /VideosController /
        [HttpGet]
        public HttpResponseMessage Get(string filename)
        {

            _filename = filename.Split('.')[0];
            _ext = filename.Split('.')[1];

            var video = new StreamFile(filename, _ext);
            var response = Request.CreateResponse();

            if (_ext.Trim().Equals("mp4"))
                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream, new MediaTypeHeaderValue("video/" + "mp4"));
            else if ((_ext.Trim().Equals("mp3")))
                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream, new MediaTypeHeaderValue("audio/" + "mp3"));
            else
                response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream, new MediaTypeHeaderValue("application/x-shockwave-flash"));
            return response;
        }





    }
}