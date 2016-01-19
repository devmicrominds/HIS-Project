using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using BinaryRage;

namespace FileMediaServer.Client
{
    

    class Program
    {
        static void Main(string[] args)
        {
            var dbPath = "C:\\Temp\\A";
            var fileName = @"Dragon Ball Z-Battle Of Gods 2013.avi";
            long fileLength = GetFileLength(fileName).Result.Value - 1;
                  
            long rem = 0;
            long batchSize =  Math.DivRem(fileLength,100,out rem);
            long fileSize = 100 * batchSize + rem;
      
            long current = 0;

            long total = 0;

            long from = 0;
            long to = 0;

            var WRITTENLENGTH = 0;

            /// TODO :
            /// Partition chunks to 10MB
            for (var i = 0; i <= 100; i++)
            {
                from = to;
                to = (i ==0) ? batchSize : from + batchSize;
                
                var result = new byte[0];

                if (i < 100)
                {
                    result = GetContent(from, to, fileName).Result;
                }
                else 
                {                    
                    to = fileLength;
                    result = GetContent(from, to, fileName).Result;
                }
                
                Console.WriteLine(result.Length);
                var len = WriteToFile(result, dbPath, fileName,from);
                WRITTENLENGTH += len;
                //Console.WriteLine(len); 
               

                //Console.WriteLine(String.Format("Index:{0} | From : {1} - To : {2} | Total Bytes : {3} ", i, from, to,to-from));
            }

            //var result = GetContent(0, fileLength,fileName).Result;
            //WriteToFile(result, dbPath, fileName);

            Console.WriteLine(WRITTENLENGTH);
            Console.ReadKey();
            //var blobRecord = new BlobRecord() { Index =  1, Blob = new byte[0] };
            //BinaryRage.DB.Insert<BlobRecord>("9", blobRecord, dbPath);
            //var b = BinaryRage.DB.Get<BlobRecord>("A",dbPath);
 
            //using (FileStream file =   new FileStream ( Path.Combine(dbPath, fileName), FileMode.Append, FileAccess.Write, FileShare.None))
            //{
            //    using (BinaryWriter bwwrite = new BinaryWriter(file))
            //    {
            //        for (var i = 0; i < 100; i++)
            //        {
            //            var q = BinaryRage.DB.Get<BlobRecord>(i.ToString(), dbPath);
            //            bwwrite.Write(q.Blob);

            //        }
            //    }
                
            //}
            
 
             
        }

        /// <summary>
        /// need to write to temp first 
        /// then append
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="dbPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int WriteToFile(byte[] byteArray,string dbPath,string fileName,long writeFrom) {
            
            int LENGTH = 0;
            using (FileStream file = new FileStream(Path.Combine(dbPath, fileName), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,4096))
            {
                LENGTH = byteArray.Length;
                
                /// Seek exact position to write                
                if(writeFrom!=file.Position)
                    file.Seek(writeFrom, SeekOrigin.Begin);

                using (BinaryWriter bwwrite = new BinaryWriter(file,System.Text.Encoding.UTF8))
                { 
                    bwwrite.Write(byteArray);
                }

            }

            return LENGTH;

        }
               
        public static async Task<byte[]> GetContent(long from, long to, string fileName)
        {
                          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/FileMediaServer/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Range = new RangeHeaderValue(from, to);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                var requestURI = @"api/MemMappedFiles/Get?filename=" + fileName;
                HttpResponseMessage response = await client.GetAsync(requestURI);
                if (response.IsSuccessStatusCode) {
                    
                    byte[] md5Hash = response.Content.Headers.ContentMD5;
                    ///
                    var size = to - from;
                    //return await response.Content.ReadAsByteArrayAsync();
                    var bytes = new byte[size];
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        // tepat skali
                        //var receivedMD5 = GetMd5Hash(stream);
                        var receivedMD5 = GetCrc(stream);
                        bool isEqual = receivedMD5.SequenceEqual(md5Hash);
                        //Console.WriteLine(isEqual); // log
                        /// if not equal then throw exception 
                        /// restart the download thread
                        /// on success append the file
                        /// 
                        
                        var bytesread = await stream.ReadAsync(bytes, 0, bytes.Length);
                        stream.Close();
                        
                    }

                    return bytes;
                }
                else
                {

                    throw new Exception("Cannot process header!");
                }
            }
        }

        private static byte[] GetMd5Hash(Stream stream)
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


        private static byte[] GetCrc(Stream stream)
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

        public static async Task<long?> GetFileLength(string fileName)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost/FileMediaServer/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Range = new RangeHeaderValue(0, 0);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                // 1.Get length
                // 2.
                var requestURI = @"api/MemMappedFiles/Head?filename=" + fileName;
                HttpResponseMessage response = await client.GetAsync(requestURI);
                if (response.IsSuccessStatusCode)
                {
                    var length = response.Content.Headers.ContentRange.Length;
                    return length;
                    //byte[] awa = await response.Content.ReadAsByteArrayAsync();
                    //Product product = await response.Content.ReadAsAsync<Product>();
                    //Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                }
                else
                {

                    throw new Exception("Cannot process header!");
                }


            }


        }
    }

    
}
