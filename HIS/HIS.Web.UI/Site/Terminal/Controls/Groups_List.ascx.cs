using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Terminal.Controls
{
    public partial class Groups_List : BaseUserControl<GroupsListPresenter, IGroupsListView>, IGroupsListView
    {    
        public GridView Grid
        {
            get
            {
                return __grid;
            }
             
        }
    }
}