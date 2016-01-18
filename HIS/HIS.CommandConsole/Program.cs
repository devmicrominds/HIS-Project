using Autofac;

using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.CommandConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            #region ASAL

            /*var bootstrap = new AutofacBootStrap();
            var factory = bootstrap.Container.Resolve<IRepositoryFactory>();

            factory.UpdateSchema();*/

            // var repository = factory.GetRepository<Repository<Campaigns>>();

            // var query = repository.GetQueryable();

            // var campaign = query.FirstOrDefault();
            // var list = new List<Timeline>();
            // list.AddRange(campaign.Timelines);

            // factory.OnTransaction(() =>
            // {
            //     foreach (var p in list)
            //         campaign.Timelines.Remove(p);
            // });


            //List<string> a = new List<string>();
            //a.Add("A");
            //a.Add("A");
            //a.Add("A");

            //Dictionary<string, string> b = new Dictionary<string, string>();
            //b.Add("A", "A");
            //b.Add("A", "A");
            //b.Add("A", "A");

            //var oa = new OABC();
            //var o = new HashSet<OABC>();
            //o.Add(oa); 
            //o.Add(oa); 
            //o.Add(oa);

            //Console.ReadKey();
            #endregion
            #region MY region
            var bootstrap = new AutofacBootStrap();
            //var factory = bootstrap.Container.Resolve<IRepositoryFactory>();
            ////MyTest.Test01 a = new MyTest.Test01(bootstrap);
            //MyTest.Test01 a = new MyTest.Test01(bootstrap);
            //a.GetDataTest();

           // string test = "ScreenTypeH1";
            //string str = test.Length < 13 ? test.Substring(11,1) : test.Substring(11,2);
           // string str2 = test.Substring(10,1);
            //create screen division h1 to h5
            DivisionCreator.ScreenCreator b = new DivisionCreator.ScreenCreator(bootstrap);
            ///
      
            #endregion
        }
    }

    public class OABC
    {

        public string Id { get; set; }
    }

    public class AutofacBootStrap
    {

        // serviceLocator
        IContainer container;


        public IContainer Container
        {
            get
            {
                if (container == null)
                {

                    ContainerBuilder builder = new ContainerBuilder();
                    OpenSessionInViewSection section = System.Configuration.ConfigurationManager.GetSection("nhibernateSettings") as OpenSessionInViewSection;
                    var localSfcp = section.SessionFactories["nhibernate"].FactoryConfigPath;

                    builder.Register<IRepositoryFactory>(x => new RepositoryFactory(localSfcp, "HIS.Data", "HIS.Data.Mapping"));
                    container = builder.Build();

                }
                return container;
            }

        }
    }
}
