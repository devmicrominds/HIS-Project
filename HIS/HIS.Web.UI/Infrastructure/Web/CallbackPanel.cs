
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public class CallbackPanel : Panel
    {

        public event EventHandler PanelRefreshed;
        public IBasePage pageContainer { get; set; }
        public IBasePage IBasePage { get; set; }

        public string ClientCallBackFunction { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
                    
        }

        protected override void OnInit(EventArgs e) {

            AddClientResource();   
            base.OnInit(e);
             
        }

        private void AddClientResource() {

            
        }

       

             

        public void RefreshPanel(Control control,params string[] callBackParms)
        {
            IBasePage = (IBasePage)this.Page;
            string HtmlToRender = string.Empty;

            IEnumerable<UserControl> controls;

            // checks if use placeholder

            var placeholder = this.Controls.OfType<PlaceHolder>().FirstOrDefault();

            if (placeholder != null)
            {
                controls = placeholder.Controls.OfType<UserControl>();
            }
            else
            {
                controls = this.Controls.OfType<UserControl>();
            }


            if (controls != null)
            {
                try
                {
                    HtmlToRender = RenderFactory.RenderUserControl(controls.First());
                    ClientIDMode mode = this.Page.ClientIDMode;
                    IBasePage.AddToRender(this.ClientID, HtmlToRender);
                    IBasePage.PanelRefresh = true;
                    if (string.IsNullOrEmpty(this.ClientCallBackFunction) == false)   {
                        IBasePage.AddCallBack(ClientCallBackFunction, callBackParms);
                    }
                }
                catch (Exception ex)
                { 
                
                }

                InvokePanelRefreshed();
            }
        }

        private void InvokePanelRefreshed()   {

            if (this.PanelRefreshed != null)
                PanelRefreshed(this, EventArgs.Empty);
        }


    }
}