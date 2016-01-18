using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Integration.Web;
using System.Web.Security;
using System.Web.UI;
using System.Security.Principal;

namespace HIS.Web.UI 
{
    public class AppSitemapProvider : StaticSiteMapProvider
    {

        RoleHelper roleHelper;


        public AppSitemapProvider() { this.roleHelper = new RoleHelper(); }

        #region Settings

        private SiteMapNode parentNode
        {
            get;
            set;
        }

        private string ExcludedFolders
        {
            get
            {
                return "(App_Data)|(obj)";
            }
        }

        private string ExcludedFiles
        {
            get
            {
                return "(Login.aspx)|(.cs)|(.Master)|(.config)|(.ascx)";
            }
        }

        #endregion

        public override SiteMapNode BuildSiteMap()
        {
            lock (this)
            {
                parentNode = HttpContext.Current.Cache["SiteMap"] as SiteMapNode;
                if (parentNode == null)
                {
                    string homeNode = HttpRuntime.AppDomainAppVirtualPath + "Site";
                    base.Clear();
                    parentNode = new SiteMapNode(this,
                                            homeNode,
                                            homeNode,
                                            "Home");

                    AddNode(parentNode);
                    AddFiles(parentNode);
                    AddFolders(parentNode);

                    HttpContext.Current.Cache.Insert("SiteMap", parentNode);
                }
                return parentNode;
            }
        }


        private void AddFolders(SiteMapNode parentNode)
        {

            var folders = from o in Directory.GetDirectories(HttpContext.Current.Server.MapPath(parentNode.Key))
                          let dir = new DirectoryInfo(o)
                          where !Regex.Match(dir.Name, ExcludedFolders).Success
                          select new
                          {
                              DirectoryName = dir.Name,
                              Key = roleHelper.DirOrder.IndexOf(dir.Name)
                          };


            var items = folders.OrderBy(x => x.Key);

            var user = HttpContext.Current.User;

            foreach (var item in items)
            {

                string role = String.Empty;

                if (roleHelper.DirectoryRoles.TryGetValue(item.DirectoryName, out role))
                {

                    //if (user.IsInRole(role))
                    //{

                        string module = item.DirectoryName.Replace('_', ' ').Split('.').Last();
                        string folderUrl = parentNode.Key + "/" + item.DirectoryName;

                        SiteMapNode folderNode =
                            new SiteMapNode(this,
                                            folderUrl,
                                            null,
                                            module,
                                            item.DirectoryName);

                        folderNode.Description = roleHelper.RoleDescription[item.DirectoryName];
                        AddNode(folderNode, parentNode);
                        AddFiles(folderNode);

                    //}
                }
            }
        }



        private void AddFiles(SiteMapNode folderNode)
        {
            var files = from o in Directory.GetFiles(HttpContext.Current.Server.MapPath(folderNode.Key))
                        let fileName = new FileInfo(o)
                        where !Regex.Match(fileName.Name, ExcludedFiles).Success
                        select new
                        {
                            FileName = fileName.Name,
                            Key = roleHelper.FileOrder.IndexOf(fileName.Name.Split('.').First())
                        };



            files = files.OrderBy(x => x.Key);


            var user = HttpContext.Current.User;


            foreach (var item in files)
            {
                // if user is in Role		
                string role = String.Empty;
                var file_name = item.FileName.Split('.').First(); // split aspx

                if (roleHelper.FileRoles.TryGetValue(file_name, out role))
                {

                    //if (user.IsInRole(role))
                    //{

                        SiteMapNode fileNode = new SiteMapNode(this,
                                            item.FileName,
                                            folderNode.Key + "/" + item.FileName,
                                            item.FileName.Replace('_', ' ').Split('.').First()
                                            );

                        AddNode(fileNode, folderNode);
                    //}

                }
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return BuildSiteMap();
        }

        public ILifetimeScope GetContainer()
        {
            var context = HttpContext.Current;
            if (context == null)
                throw new Exception("No HttpContext");

            var accessor = context.ApplicationInstance as IContainerProviderAccessor;
            if (accessor == null)
                throw new Exception("The global HttpApplication instance needs to implement " + typeof(IContainerProviderAccessor).FullName);
            if (accessor.ContainerProvider.ApplicationContainer == null)
                throw new Exception("HttpApplication has no container initialized");

            return accessor.ContainerProvider.ApplicationContainer;
        }
    }
}