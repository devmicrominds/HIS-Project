using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.Test
{

    public class jsonTestPresenter : GenericPresenter<IjsonTestView>
    {
        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public jsonTestPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            BuildRunActions();

        }

        public override void InitialRun()
        {
            base.InitialRun();
            GetData();
            View.AddControl(ControlPath.IJsonTest, View.ControlPlaceHolder);

        }
        private void GetData()
        {



            var repository = factory.GetRepository<Repository<Resource>>();
            var query = repository.GetQueryable();

            var filterquery = query.Where(o => o.Name.Contains("Iklan")).ToList();
            int cnn = 0;
            var result = filterquery.Select(o => new
            {
                ID = o.Id,
                bdet = new
                {
                    ID = o.Id,
                    SEQNO = cnn++,
                    Name = o.Name,
                }

            }).ToDictionary(o => o.ID, o => o.bdet);





            View.JsonData = Json(result);

        }



        public override void PartialRender()
        {
            string action = View.PostParameter.Action;
            RunAction[action]();

        }

        private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
                 { "click01",click1 },

                 
            };
        }



        private void click1()
        {
            var p = View.PostParameter;
            var data = p.Data;


        }

    }
}