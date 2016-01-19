using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMediaServer.Client
{
    [Serializable]
    public class BlobRecord
    {
        public int Index { get; set; }

        public List<byte[]> Blob { get; set; }

        public void AddBlob(byte[] bytes) {

            if (null == Blob)
                Blob = new List<byte[]>();

            Blob.Add(bytes);
        }
    }
}
