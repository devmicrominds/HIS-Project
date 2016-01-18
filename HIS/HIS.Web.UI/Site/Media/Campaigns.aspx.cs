using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Media
{
    public partial class Campaigns : BasePage<CampaignsPresenter,ICampaignsView>,ICampaignsView   {

        public CallbackPanel ListPanel { get; set; }
        public PlaceHolder ListPlaceHolder { get; set; }
        
    }
}