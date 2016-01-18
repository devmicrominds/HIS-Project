using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class HTML5Icon
    {
        private byte[] o_icon = new byte[0];
        
        public HTML5Icon(byte[] bytes)
        {
            o_icon = bytes;
        }


        public byte[] Icon
        {
            get {
                return o_icon; 
            }
        }
    }
}