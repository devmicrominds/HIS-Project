using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace HIS.Web.UI 
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Contents/lib/jquery/jquery-2.1.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/javascripts").Include(
                     
                     "~/Contents/lib/bootstrap/transition.js",
                     "~/Contents/lib/bootstrap/collapse.js",  
                     "~/Contents/lib/bootstrap/alert.js",  
                     "~/Contents/lib/bootstrap/tooltip.js",  
                     "~/Contents/lib/bootstrap/popover.js", 
                     "~/Contents/lib/bootstrap/button.js",  
                     "~/Contents/lib/bootstrap/tab.js", 
                     "~/Contents/lib/bootstrap/dropdown.js",
                     "~/Contents/lib/backbone/underscore-min.js"
                     
                     ));

            bundles
                .Add(new StyleBundle("~/Contents/css")
                .Include("~/Contents/css/application.css")
                )
                ;
        }
    }
}