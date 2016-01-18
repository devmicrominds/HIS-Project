using Autofac;
using Autofac.Integration.Web;
using HIS.Web.UI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace HIS.Web.UI.Web
{

    /// <summary>
    /// Base Page with MVP model
    /// </summary>
    /// <typeparam name="TPresenter"></typeparam>
    /// <typeparam name="TView"></typeparam>
    public abstract class BasePage<TPresenter, TView> : Page, IWebControl, IPageView
        where TPresenter : GenericPresenter<TView>
        where TView : class, IView
    {

        private const string ViewStateModeSetting = "ViewStateMode";
        private const string ViewStateSessionId = "ViewStateSessionId";
        private const string CompressedViewStateId = "CompressedViewState";
        private dynamic jsonModel;


        public TPresenter Presenter { get; set; }

        public BasePage()
        {

            InitPresenter();
        }

        private void InitPresenter()
        {
            var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
            var cp = cpa.ContainerProvider;
            cp.RequestLifetime.InjectProperties(this);
            this.Presenter.BindToView(this as TView);

        }

        protected override void OnInit(EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                ILifetimeScope scope = (ILifetimeScope)HttpContext.Current.Session["sessionScope"];
                if (null != scope)
                {
                    ISessionProvider sessionProvider = scope.Resolve<ISessionProvider>();
                    if (null != sessionProvider)
                    {
                        Presenter.BindSessionProvider(sessionProvider);
                    }
                    else
                    {
                        HttpContext.Current.Response.Redirect("~/Login.aspx");
                    }
                }
            }

            this.Presenter.Initialize();
            IsPartialRendering = false;

            if (Request.Form["__parameters"] != null)
            {
                IsPartialRendering = true;
                this.PostParameter = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Request.Form["__parameters"]);
            }
            else
            {

                if (Request.Form.Count != 0)
                    this.PostParameter = NVCHelper.ToDictionary(Request.Form);
            }

            this.Presenter.Process();



            base.OnInit(e);

        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);



        }


        /// <summary>
        /// Called after a child control is added to the <see cref="P:System.Web.UI.Control.Controls"></see> collection of the <see cref="T:System.Web.UI.Control"></see> object.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"></see> that has been added.</param>
        /// <param name="index">The index of the control in the <see cref="P:System.Web.UI.Control.Controls"></see> collection.</param>
        protected override void AddedControl(Control control, int index)
        {

            base.AddedControl(control, index);

            if (IsCreateHierarchy)
            {
                foreach (IWebControl c in this.FindMVPChildren(control))
                {
                    c.Hierarchize(this.Presenter);
                }
            }

        }

        /// <summary>
        /// Called after a control is removed from the <see cref="P:System.Web.UI.Control.Controls"></see> collection of another control.
        /// </summary>
        /// <param name="control">The <see cref="T:System.Web.UI.Control"></see> that has been removed.</param>
        protected override void RemovedControl(Control control)
        {
            base.RemovedControl(control);

            // suppose we have 
            // <res:c ID="parent">
            //  <div>
            //   <res:c ID="child">
            // the current event is rised in the parent just for the div
            // so we have to find the child so not to loose it
            if (IsCreateHierarchy)
            {
                foreach (IWebControl c in this.FindMVPChildren(control))
                {
                    c.Hierarchize(null);
                }
            }
        }



        private IEnumerable<IWebControl> FindMVPChildren(Control control)
        {
            if (null != control)
            {
                if (control is IWebControl)
                {
                    yield return (control as IWebControl);
                }
                else if (0 < control.Controls.Count)
                {
                    foreach (Control childControl in control.Controls)
                    {
                        foreach (IWebControl c in this.FindMVPChildren(childControl))
                        {
                            yield return c;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the presenter of the current view instance.
        /// </summary>
        /// <value>The presenter of the current view instance.</value> 

        #region IWebControl Members

        /// <summary>
        /// Hierarchizes the current instance into the hierarchy of the specified presenter.
        /// </summary>
        /// <param name="shouldBeParent">The logical parent presenter.</param>
        public void Hierarchize(Presenter shouldBeParent)
        {
            this.Presenter.Parent = shouldBeParent;
        }

        #endregion

        #region IBasePage Members

        private bool isPartialRendering = false;
        private StringBuilder responseToRender = new StringBuilder();


        public bool IsPartialRendering
        {
            get
            {
                return isPartialRendering;
            }
            set
            {
                isPartialRendering = value;
            }
        }

        public StringBuilder ResponseToRender
        {
            get
            {
                return responseToRender;
            }
        }

        public void AddToRender(string panelId, string htmlToAdd)
        {

            //that's is just to avoid sitax error if there is javascript on the control that is gonna be rendered
            //Must be improved/refactored
            htmlToAdd = htmlToAdd.Replace("\\", "\\\\");
            htmlToAdd = htmlToAdd.Replace(Environment.NewLine, string.Empty);
            htmlToAdd = htmlToAdd.Replace(@"""", "\\\"");
            this.ResponseToRender.Append(@"$(""#" + panelId + @""").html(""" + htmlToAdd + @""");");
        }

        public void AddCallBack(string functionName, params string[] callBackParms)
        {

            StringBuilder paramToJs = new StringBuilder();
            if (callBackParms.Length > 0)
            {
                foreach (string parm in callBackParms)
                {
                    paramToJs.Append("'" + parm + "',");
                }
                paramToJs.Remove(paramToJs.Length - 1, 1);
            }
            this.ResponseToRender.Append(functionName + "(" + paramToJs.ToString() + ");");
        }

        public bool PanelRefresh { get; set; }


        public dynamic PostParameter
        {
            get;
            set;
        }

        public dynamic JsonData
        {
            get { return jsonModel ?? new { }; }
            set { jsonModel = value; }
        }

        // Methods
        protected override void Render(HtmlTextWriter writer)
        {
            if (PanelRefresh)
            {

                Response.Write(this.ResponseToRender.ToString());
                Response.End();
            }

            base.Render(writer);



        }




        #endregion

        private bool isCreateHierarchy = true;

        public bool IsCreateHierarchy
        {
            get { return isCreateHierarchy; }
            set { isCreateHierarchy = value; }
        }

        public ISessionProvider SessionProvider
        {

            get
            {
                return Presenter.SessionProvider;
            }
        }

        public IBasePage Parent
        {
            get
            {

                return (IBasePage)this;
            }
        }

        public Control AddControl(string path, Control placeHolder)
        {
            placeHolder.Controls.Clear();
            UserControl uc = (UserControl)LoadControl(path);
            if (uc is IWebControl)
            {
                IWebControl c = (IWebControl)uc;
                this.Presenter.BindChildren(c);
            }
            placeHolder.Controls.Add(uc);
            return uc;
        }

        public string GetControlPath(string relativePath)     {

            return VirtualPathUtility.Combine(this.AppRelativeTemplateSourceDirectory, relativePath);

        }

        public virtual PageSettings LocalPageSettings { get; set; }

        #region ViewState Management

        private ViewStateConfig _viewStateMode = ViewStateConfig.Session;
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ViewStateConfig.NotSet)]
        public ViewStateConfig _ViewStateMode
        {
            get
            {

                //The setting on the page overrdes the one on the site config.
                if (_viewStateMode != ViewStateConfig.NotSet)
                {
                    return _viewStateMode;
                }
                else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[ViewStateModeSetting]))
                {
                    return (ViewStateConfig)Enum.Parse(typeof(ViewStateConfig), ConfigurationManager.AppSettings[ViewStateModeSetting], true);
                }
                else
                {
                    return ViewStateConfig.Default;
                }
            }
            set
            {
                _viewStateMode = value;
            }
        }

        protected override void SavePageStateToPersistenceMedium(object state)
        {
            switch (_ViewStateMode)
            {
                case ViewStateConfig.Session:
                    //Since the session isn't always availible, we had better make sure we
                    //do something sensible
                    if (this.Session.Mode != SessionStateMode.Off)
                    {
                        this.SaveToSession(state);
                    }
                    else
                    {
                        this.CompressViewstate(state);
                    }
                    break;
                case ViewStateConfig.Compress:
                    this.CompressViewstate(state);
                    break;
                default:
                    base.SavePageStateToPersistenceMedium(state);
                    break;
            }
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            switch (_ViewStateMode)
            {
                case ViewStateConfig.Session:
                    if (this.Session.Mode != SessionStateMode.Off)
                    {
                        return this.LoadFromSession();
                    }
                    else
                    {
                        return this.DecompressViewstate();
                    }
                case ViewStateConfig.Compress:
                    return this.DecompressViewstate();
                default:
                    return base.LoadPageStateFromPersistenceMedium();
            }
        }

        private void SaveToSession(object state)
        {
            //First lets see if there is already a session id availible to us
            string viewStateSessionId = base.Request.Form[ViewStateSessionId];
            if (string.IsNullOrEmpty(viewStateSessionId))
            {
                //If there isn't then we'll be needing one.
                viewStateSessionId = Guid.NewGuid().ToString();
            }

            //Save the data into our session object
            Session[viewStateSessionId] = state;

            //Lastly we save the sessionid for when the page is loaded.
            Page.ClientScript.RegisterHiddenField(ViewStateSessionId, viewStateSessionId);
        }

        private object LoadFromSession()
        {
            return Session[base.Request.Form[ViewStateSessionId]];
        }

        private void CompressViewstate(object state)
        {
            //The ObjectStateFormatter is explicitly for serializing
            //viewstate, if you're using .net 1.1 then use the LosFormatter

            //First off, lest gets the state in a byte[]
            ObjectStateFormatter formatter = new ObjectStateFormatter();
            byte[] bytes;
            using (MemoryStream writer = new MemoryStream())
            {
                formatter.Serialize(writer, state);
                bytes = writer.ToArray();
            }

            //Now we've got the raw data, lets squish the whole thing
            using (MemoryStream output = new MemoryStream())
            {
                using (DeflateStream compressStream = new DeflateStream(output, CompressionMode.Compress, true))
                {
                    compressStream.Write(bytes, 0, bytes.Length);
                }

                //OK, now lets store the compressed data in a hidden field.
                Page.ClientScript.RegisterHiddenField(CompressedViewStateId, Convert.ToBase64String(output.ToArray()));
            }
        }

        private object DecompressViewstate()
        {
            //First lets get ths raw compressed string into a byte[]
            byte[] bytes = Convert.FromBase64String(Request.Form[CompressedViewStateId]);

            using (MemoryStream input = new MemoryStream(bytes))
            {
                //Now push the compressed data into the decompression stream
                using (DeflateStream decompressStream = new DeflateStream(input, CompressionMode.Decompress, true))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        //Now we wip through the decompression stream and pull our data back out
                        byte[] buffer = new byte[256];
                        int data;
                        while ((data = decompressStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, data);
                        }

                        //Finally we convert the whole lot back into a string and convert it
                        //back into it's original object.
                        ObjectStateFormatter formatter = new ObjectStateFormatter();
                        return formatter.Deserialize(Convert.ToBase64String(output.ToArray()));
                    }
                }
            }
        }

        #endregion
    }

    public enum ViewStateConfig
    {
        Session,
        Compress,
        Default,
        NotSet
    }
}