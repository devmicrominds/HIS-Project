using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class CampaignsLayoutSelectorPresenter : GenericPresenter<ICampaignsLayoutSelectorView>
    {
        public CampaignsLayoutSelectorPresenter(IRepositoryFactory factory) { 
        
        }

        public override void View_Load(object sender, EventArgs e)
        {
            View.JsonData = Json(View.PostParameter);
        }
    }
}