using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Terminal.Controls
{
    public partial class Players_List : BaseUserControl<PlayersListPresenter, IPlayersListView>, IPlayersListView
    {

        public GridView Grid
        {
            get
            {
                return __grid;
            }

        }

        public dynamic Model { get; set; }

        public override PageSettings LocalPageSettings
        {
            get
            {

                return new PageSettings(Grid.PageSize, Grid.PageIndex);
            }
            set
            {

                Grid.PageIndex = value.PageIndex;
                Grid.PageSize = value.PageSize;

            }
        }

    }
}