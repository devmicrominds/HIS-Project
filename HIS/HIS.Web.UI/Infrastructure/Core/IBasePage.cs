using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace HIS.Web.UI 
{
    public interface IBasePage   : IView
    {         
        bool IsPostBack { get; }
        bool IsPartialRendering { get; set; }
        StringBuilder ResponseToRender { get; }
        Control AddControl(string path, Control placeHolder);
        void AddToRender(string panelId, string htmlToAdd);
        void AddCallBack(string functionName, params string[] callBackParms);
        bool PanelRefresh { get; set; }
        string GetControlPath(string relativePath);
        Control LoadControl(string path);

        
    }
}
