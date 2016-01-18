using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace HIS.Web.UI 
{
    public interface IView
    {
        IBasePage Parent { get; }
        dynamic JsonData { get; set; }
        dynamic PostParameter { get; set; }

        event EventHandler Load;
        PageSettings LocalPageSettings { get; set; }
    }
}