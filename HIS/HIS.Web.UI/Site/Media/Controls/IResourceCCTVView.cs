using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Media.Controls
{
    public interface IResourceCCTVView : IView
    {
        dynamic Model { get; set; }
        GridView Grid { get; }
    }
}
