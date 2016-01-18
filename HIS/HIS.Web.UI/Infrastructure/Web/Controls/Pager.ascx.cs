using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public partial class Pager : UserControl
    {
        protected GridView _gridView;

        protected void Page_Load(object sender, EventArgs e)   {    }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Control c = Parent;
            while (c != null)
            {
                if (c is GridView)
                {
                    _gridView = (GridView)c;
                    break;
                }
                c = c.Parent;
            }

            _gridView.DataBound += _gridView_DataBound;
          
        }
        
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (_gridView != null)
            {
                LabelNumberOfPages.Text = _gridView.PageCount.ToString(System.Globalization.CultureInfo.CurrentCulture);
                LoadPageIndexItems();
                DropDownPageIndex.SelectedIndex = _gridView.PageIndex;
                DropDownPageSize.SelectedValue = _gridView.PageSize.ToString(System.Globalization.CultureInfo.CurrentCulture);

                
            }
        }

        protected void _gridView_DataBound(object sender, EventArgs e)
        {
            if (_gridView.TopPagerRow != null) 
                _gridView.TopPagerRow.Visible = true;
           
        }

        private void LoadPageIndexItems()
        {
            for (int i = 0; i < _gridView.PageCount; i++)
            {
                DropDownPageIndex.Items.Add(new ListItem()
                {
                    Text = String.Format("Page {0} &nbsp;&nbsp;", (i + 1).ToString()) ,
                    Value = i.ToString()
                });
            }

        }

        public string PagerContext { get; set; }

        public string VirtualPath
        {
            get {
                
                return _gridView.Parent.TemplateControl.AppRelativeVirtualPath;
            }
        }

    }
}