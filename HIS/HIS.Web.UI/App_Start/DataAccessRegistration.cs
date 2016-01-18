using Autofac;
using HIS.Data;
using HIS.Data.Mapping.Utils;
using HIS.DataAccess;
using HIS.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.App_Start
{
    public class DataAccessRegistration  : Module
    { 
        protected override void Load(ContainerBuilder builder)
        {
            LocalDataConfiguration(builder);
        }

        private void LocalDataConfiguration(ContainerBuilder builder) {

            OpenSessionInViewSection openSessionInViewSection = System.Configuration.ConfigurationManager
             .GetSection("nhibernateSettings") as OpenSessionInViewSection;

            string localPath = openSessionInViewSection.SessionFactories["nhibernate"].FactoryConfigPath;
            string assemblyName = openSessionInViewSection.SessionFactories["nhibernate"].AssemblyName;
            string mappingAssembly = openSessionInViewSection.SessionFactories["nhibernate"].MappingAssembly;

            builder.Register(c => new RepositoryFactory(localPath, assemblyName, mappingAssembly))
                   .SingleInstance()
                   .AsImplementedInterfaces();

            // new instance everytime
            builder.Register<IPathLocatorResolver>(c => new PathLocatorResolver(c.Resolve<IRepositoryFactory>().OpenSession()))
                   .AsImplementedInterfaces();
                   
          
        
        }
    }
}