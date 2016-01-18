using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace HIS.Web.UI
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {     
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path) {}

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers) {
        
            var extension =  headers.ContentDisposition.FileName.Split('.').Last();
            var fileName = String.Format("{0}.{1}", GenerateComb().ToString(), extension);  

            return fileName.Replace("\"", string.Empty);
        }

        

        

        private Guid GenerateComb()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();
            DateTime baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            // Get the days and milliseconds which will be used to build the byte string
            TimeSpan days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;
            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));
            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);
            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);
            return new Guid(guidArray);
        }

        
    }
}