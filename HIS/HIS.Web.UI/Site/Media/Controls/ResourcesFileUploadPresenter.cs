using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class ResourcesFileUploadPresenter  : GenericPresenter<IResourcesFileUploadView>
    {
        public ResourcesFileUploadPresenter() { }

        public override void View_Load(object sender, EventArgs e)
        {
            View.JsonData = View.PostParameter;    
        }
    }
}