using Autofac;
using Autofac.Integration.Web;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;

namespace HIS.Web.UI
{
    public abstract class BaseUserControl<TPresenter, TView> : UserControl, IWebControl
        where TPresenter : GenericPresenter<TView>
        where TView : class, IView
    {

        private dynamic jsonModel;

        public string ViewURL
        {

            get
            {
                return this.Request.Url.AbsolutePath;
            }
        }

        public virtual string ContentPlaceHolderId { get; set; }

        public virtual TPresenter Presenter { get; set; }

        public virtual IPrincipal User { get; set; }
        // Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUserControl&lt;TPresenter, TView&gt;"/> class.
        /// </summary>
        public BaseUserControl()
        {

            if (this.Presenter == null)
            {

                var currentContext = HttpContext.Current;
                this.User = currentContext.User;

                var cpa = (IContainerProviderAccessor)HttpContext.Current.ApplicationInstance;
                var cp = cpa.ContainerProvider;
                cp.RequestLifetime.InjectProperties(this);
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


            this.Presenter.BindToView(this as TView);
        }


        protected override void OnInit(EventArgs e)
        {
            this.Presenter.Initialize();
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// 

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

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.Presenter.Process();

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


        // Properties

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

        #region IWebControl Members


        private bool isCreateHierarchy = true;

        public bool IsCreateHierarchy
        {
            get { return isCreateHierarchy; }
            set { isCreateHierarchy = value; }
        }

        #endregion

        public ISessionProvider SessionProvider
        {

            get
            {

                return Presenter.SessionProvider;
            }
        }

        public dynamic PostParameter
        {
            get { return Parent.PostParameter; }
            set { }

        }

        public dynamic JsonData
        {
            get 
            {
                return jsonModel ?? new { };
            }
            set {
                
                jsonModel = value;
            }
        }

        public IBasePage Parent
        {
            get { return (IBasePage)Presenter.Parent.View; }
        }

        public virtual PageSettings LocalPageSettings { get; set; }
    }
}