using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public interface IPlayersListView : IWebControl
    {
        GridView Grid { get; }
        dynamic Model { get; set; }
    }
}
