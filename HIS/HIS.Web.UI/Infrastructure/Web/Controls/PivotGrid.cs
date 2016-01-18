using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI 
{

    [ToolboxData("<{0}:PivotGridView runat=server></{0}:PivotGridView>")]
    public class PivotGridView : GridView
    {
        bool _pivotGrid = true;

        [Browsable(true)]
        public bool PivotGrid
        {
            get
            {
                return _pivotGrid;
            }
            set
            {
                _pivotGrid = value;
                EnsureChildControls();
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (_pivotGrid)
            {
                System.IO.TextWriter baseOutputTextWriter = new System.IO.StringWriter();
                HtmlTextWriter baseOutputHtmlWriter = new HtmlTextWriter(baseOutputTextWriter);

                base.RenderContents(baseOutputHtmlWriter);

                output.Write(HtmlParserService.PivotHtmlTableMarkup(baseOutputTextWriter.ToString()));
            }
            else
            {
                base.RenderContents(output);
            }
        }
    }
}