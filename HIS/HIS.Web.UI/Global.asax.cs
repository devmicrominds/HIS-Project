using Autofac;
using Autofac.Integration.Web;
using HIS.Web.UI.App_Start;   
using HIS.Web.UI.Site;
using System;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Autofac.Integration.WebApi;
using System.Web.Optimization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using System.Web.Routing;
using System.Web.Http.Hosting;
using HIS.Shared;
using Newtonsoft.Json.Serialization;
 

namespace HIS.Web.UI
{
    public class Global : System.Web.HttpApplication, IContainerProviderAccessor
    {
        static IContainerProvider containerProvider;

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            Assembly assembly = Assembly.GetAssembly(typeof(HomePresenter));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Presenter"))
                .AsSelf()
                .InstancePerHttpRequest();

             builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => 
                  !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t))
               .InstancePerApiRequest();

            builder.Register(x => new WebSessionProvider()).As<ISessionProvider>()
                   .InstancePerMatchingLifetimeScope("session")
                   .CacheInSession();
             
            builder.RegisterType(typeof(MD5SaltEncryption));

            builder.RegisterType(typeof(AppMembershipProvider))
                .As<MembershipProvider>()
                .Keyed<MembershipProvider>("myProvider")
                .InstancePerLifetimeScope();

            var html5icon = HTML5IconThumb();
            builder.Register(c => new HTML5Icon(html5icon))
                   .AsSelf()
                   .SingleInstance();

            builder.Register(c => new Icons(c.Resolve<HTML5Icon>()))
                .AsSelf()
                .SingleInstance();

            builder.Register(c => new ApplicationPath(Properties.Settings.Default.ServerUploadRootPath))
                .As<ApplicationPath>()
                .SingleInstance();

            builder.RegisterModule<DataAccessRegistration>();

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            containerProvider = new ContainerProvider(container);

            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHostBufferPolicySelector), new NoBufferPolicySelector());
            var serializerSettings =    GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            var contractResolver =    (DefaultContractResolver)serializerSettings.ContractResolver;
            contractResolver.IgnoreSerializableAttribute = true;
            RegisterRoute(RouteTable.Routes);    

        }

        private byte[] HTML5IconThumb()
        {
            var icon = System.IO.File.ReadAllBytes(Server.MapPath("~/Contents/img/html5.png"));
            return (icon != null) ? icon: new byte[0];
        }

        private void RegisterRoute(RouteCollection routes) {

            routes.MapPageRoute("Administration", "Accounts/{Name}", "~/Site/Administration/Accounts.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            var sessionScope = containerProvider.RequestLifetime.BeginLifetimeScope("session");
            Session["sessionScope"] = sessionScope;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
            
            System.Threading.Thread.CurrentThread.CurrentUICulture  = new System.Globalization.CultureInfo("EN-my");
            var oTime = DateTime.Now;
        }
        
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        //string userData =GetUserRoles();
                        //string [] roles = userData.Split (',');

                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);
                    }
                }
            }
             
        }

        protected void Application_EndRequest(object sender, EventArgs e) {

            if (HttpContext.Current == null)
            {
                var status = Response.Status;
                //Response.Flush();
                Response.End();
                //Response.Redirect(FormsAuthentication.LoginUrl, true);
            }
            else
            {

                if (HttpContext.Current.Response.StatusCode == 401)
                    Response.Redirect(FormsAuthentication.LoginUrl, true);
            }
            

            

            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            var sessionScope = (ILifetimeScope)Session["sessionScope"];
            if (null != sessionScope)
                sessionScope.Dispose();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        public IContainerProvider ContainerProvider
        {
            get
            {
                return containerProvider;
            }
        }
    }
}