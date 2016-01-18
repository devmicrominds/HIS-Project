using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    [Serializable]
    public class PageSettings
    {
        private int pageIndex = 0;
        private int pageSize = 10;
        private string controlVirtualPath = "";

        public PageSettings(int pageIndex = 0, int pageSize = 10,string controlVirtualPath="")
        {
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
            this.controlVirtualPath = controlVirtualPath;
          
        }

        public int PageIndex
        {
            get { return pageIndex; }
        }

        public int PageSize
        {
            get { return pageSize; }
        }

        public string PageContext { get; set; }

        public static PageSettings Default() {

            return new PageSettings();
           
        }
    }
}