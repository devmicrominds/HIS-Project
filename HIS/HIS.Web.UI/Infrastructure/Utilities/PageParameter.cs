using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Utilities
{
    public class PageParameter
    {
        public static IDictionary<string, string> PopulatePageParameter(string __parameters)
        {

            var postParameters = new Dictionary<string, string>();

            try
            {

                string[] parameters = __parameters.Split('&');

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (string.IsNullOrEmpty(parameters[i]) == false)
                    {
                        int equalPosition = parameters[i].IndexOf("=");
                        if (parameters[i].Contains("[]"))
                        {
                            if (!postParameters.ContainsKey(parameters[i].Substring(0, equalPosition)))
                            {
                                postParameters.Add(parameters[i].Substring(0, equalPosition), parameters[i].Substring(equalPosition + 1));
                            }
                            else
                            {
                                var temp = postParameters[parameters[i].Substring(0, equalPosition)];
                                temp = String.Format("{0},{1}", temp, parameters[i].Substring(equalPosition + 1));
                                postParameters[parameters[i].Substring(0, equalPosition)] = temp;
                            }
                        }
                        else
                        {
                            postParameters.Add(parameters[i].Substring(0, equalPosition), parameters[i].Substring(equalPosition + 1));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new FormatException("The parameters string is not in the correct format", ex);
            }

            return postParameters;
        }
    }
}