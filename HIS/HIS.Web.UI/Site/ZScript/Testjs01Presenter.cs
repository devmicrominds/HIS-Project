using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI.Site.ZScript
{
    public class Testjs01Presenter : GenericPresenter<ITestjs01View>
    {
        private IRepositoryFactory factory;
        private IDictionary<string, Action> RunAction;

        public Testjs01Presenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            BuildRunActions();
            //EditCampaign();
          //  ScreenTemplates();
        }

        public override void InitialRun()
        {
            base.InitialRun();
            ScreenTemplates();
            View.AddControl(ControlPath.ITestjs01Settings, View.ControlPlaceHolder);
           
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
       

        private void ScreenTemplates()
        {

            var screenTemplate = factory.GetRepository<Repository<ScreenOrientation>>();

            var sQuery = screenTemplate.GetQueryable();

            var list = sQuery.ToList();

            var result = list
                         .Select(o => new
                         {
                             or = o.Orientation,
                             s = o.ScreenResolutions
                                  .Select(w => new
                                  {
                                      resolution = w.Resolution,
                                      screenTypes = w.ScreenTypes
                                                     .Select(u => new
                                                     {
                                                         u.Id,
                                                         sd = u.ScreenDivisions
                                                               .Select(v =>
                                                                   new
                                                                   {
                                                                       sdn = v.Name,
                                                                       sdd = new
                                                                       {
                                                                           id = v.Id,
                                                                           x = v.X,
                                                                           y = v.Y,
                                                                           w = v.Width,
                                                                           h = v.Height
                                                                       }
                                                                   })
                                                               .ToDictionary(f => "sd" + f.sdn,
                                                                             f => f.sdd)

                                                     }).ToDictionary(z => z.Id,
                                                                     z => z.sd)

                                  }).ToDictionary(r => r.resolution,
                                                  r => r.screenTypes)

                         }).ToDictionary(t => t.or, t => t.s);





            View.JsonData = Json(result);

        }
      



    }
}