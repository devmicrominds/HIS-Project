using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class ResourcesPresenter  : GenericPresenter<IResourcesView>
    {
        public override void InitialRun()
        {
            base.InitialRun();
            var childView = (IResourcesMenuView)View.AddControl(ControlPath.IResourcesMenuView, View.ControlPlaceHolder);
             
            childView.IsInitialRun = true;
             
            
        }

        public override void PartialRender()
        {
            View.AddControl(ControlPath.IResourcesMenuView, View.ControlPlaceHolder);
            View.ControlPanel.RefreshPanel(View.ControlPlaceHolder);
            
        }
    }
}