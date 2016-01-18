using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class Strings
    {
        public static string UnquoteToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
                return token;

            if (token.StartsWith("\"", StringComparison.Ordinal) && token.EndsWith("\"", StringComparison.Ordinal) && token.Length > 1)
                return token.Substring(1, token.Length - 2);

            return token;
        }
    }
}