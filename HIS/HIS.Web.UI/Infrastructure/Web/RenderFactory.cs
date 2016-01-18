using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HIS.Web.UI
{
    public class RenderFactory
    {

        public static string RenderUserControl(Control crtl)
        {         
            const string STR_BeginRenderControlBlock = "<!-- BLOCK RENDER CONTROL -->";
            const string STR_EndRenderControlBlock = "<!-- END RENDERCONTROL -->";
            StringWriter tw = new StringWriter();
            Page page = new Page();

          
            HtmlForm form = new HtmlForm();
            form.ID = "__temporanyForm";
            page.Controls.Add(form);
            form.Controls.Add(new LiteralControl(STR_BeginRenderControlBlock));           
            form.Controls.Add(crtl);
            form.Controls.Add(new LiteralControl(STR_EndRenderControlBlock));
            HttpContext.Current.Server.Execute(page, tw, true);
      
            string Html = tw.ToString();
            //TO DO:clean the response!!!!!
            int start = Html.IndexOf("<!-- BLOCK RENDER CONTROL -->");
            int end = Html.Length - start;
            Html = Html.Substring(start, end);
            return Html;

        }

       

       


    }
}