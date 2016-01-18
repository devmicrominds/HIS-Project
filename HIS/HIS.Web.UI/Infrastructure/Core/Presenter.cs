using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace HIS.Web.UI 
{
    public abstract class Presenter  
    {
        protected IView view;
        protected Presenter parent;
        protected IList<Presenter> children;
        protected bool initialized = false;
        protected ISessionProvider sessionProvider;
              
        public event EventHandler ParentChanged;
        
        
        public virtual Presenter BindToView(IView view)
        {
            if (null != this.view)
            {
                throw new InvalidOperationException("The presenter is already bound.");
            }

            this.view = view;

            if (null == this.view)
            {
                throw new ArgumentNullException("view");
            }

            ViewBinded();
            return this;
        }
        
        
        public virtual void ViewBinded() {

            View.Load += View_Load;
            
        }

       
        public virtual void BindSessionProvider(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

   
        public virtual Presenter Initialize()
        {
            if (this.initialized)
            {
                throw new InvalidOperationException("Cannot initialize twice.");
            }
            this.initialized = true;

            return this;
        }

     
        public virtual Presenter Process()
        {
            return this;
        }

   
        public virtual void BindChildren(IWebControl c)
        {
            c.Hierarchize(this);
        }

         
        public virtual Presenter Parent
        {
            get { return this.parent; }
            set
            {
                if (value != this.parent)
                {
                    if (null != this.parent)
                    {
                        this.parent.Children.Remove(this as Presenter);
                    }

                    this.parent = value;

                    if (null != this.parent)
                    {
                        this.parent.Children.Add(this as Presenter);
                    }

                    if (null != this.ParentChanged)
                    {
                        this.ParentChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

         
        public Presenter Root
        {
            get { return (null == this.parent) ? this : this.parent.Root; }
        }

         
        public IList<Presenter> Children
        {
            get
            {
                if (null == this.children)
                {
                    this.children = new List<Presenter>();
                }
                return this.children;
            }
        }    
         
        internal protected IView ViewInternal
        {
            get
            {
                // ensure presenter binding even with a small 
                // performace penalty cost due to view instance checking
                if (null == this.view)
                {
                    throw new InvalidOperationException("Use of unbound presenter.");
                }
                return this.view;
            }
        }
                   
        public IView View
        {

            get
            {

                if (null == this.view)
                {
                    throw new InvalidOperationException("Use of unbound presenter.");
                }
                return this.view;
            }
        }
         
        public ISessionProvider SessionProvider { 
            get {
                return sessionProvider; 
            } 
        }

        #region Init

        public virtual void PartialRender() {

            string action = View.PostParameter.Action;
            switch (action) {
                case "PageSettingsChanged":
                    string pageContext = View.PostParameter.PagerContext;
                    PageSettingsList[pageContext] = new PageSettings((int)View.PostParameter.PageIndex, (int)View.PostParameter.PageSize);
                    
                    break;
            
            }
        
        }

        public virtual void PostBack() {}

        // Run on first time
        public virtual void InitialRun() {
            
            ClearPageSettings();
        }

        public void ClearPageSettings() {

            
            this.PageSettingsList = new Dictionary<string,PageSettings>();
        }

        public IDictionary<string,PageSettings> PageSettingsList {

            get {

                if (!SessionProvider.Contains("page.settings.list"))
                {
                    SessionProvider["page.settings.list"] = new Dictionary<string, PageSettings>();
                }
                return (IDictionary<string, PageSettings>)SessionProvider["page.settings.list"];
            }
            set {
                
                SessionProvider["page.settings.list"] = value;
            }

        }

        
        

        #endregion

        public virtual void View_Load(object sender, EventArgs e)
        {
            if (View is IBasePage)
            {
                var _basePage = (IBasePage)View;

                if (_basePage.IsPartialRendering)
                    PartialRender();

                if (_basePage.IsPostBack)
                    PostBack();

                if (!_basePage.IsPostBack && !_basePage.IsPartialRendering)
                    InitialRun();
            }
        }

        public string Json(dynamic param) {

            return Newtonsoft.Json.JsonConvert.SerializeObject(param,Newtonsoft.Json.Formatting.Indented);
        }
        
    }
}