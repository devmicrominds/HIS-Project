using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Utilities
{
    public static class NVCHelper
    {
        public static IDictionary<string, string> ToDictionary
            (this NameValueCollection source)
        {
            //return source.Cast<string>()  
            //			 .Select(s => new { Key = s, Value = source[s] })  
            //			 .ToDictionary(p => p.Key, p => p.Value); 
            IDictionary<string, string> result = new Dictionary<string, string>();
            foreach (string k in source)
            {

                if (k != null)
                {
                    var u = source.GetValues(k);
                    if (u != null)
                    {

                        result.Add(k, source[k]);
                    }


                }
            }

            return result;
        }
    }
}