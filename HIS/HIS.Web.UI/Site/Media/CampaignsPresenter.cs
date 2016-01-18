using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class CampaignsPresenter : GenericPresenter<ICampaignsView>
    {

        private IRepositoryFactory factory;

        public CampaignsPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
        }

        public override void InitialRun()
        {
            base.InitialRun();
            ScreenTemplates();
            View.AddControl(ControlPath.ICampaignsListView, View.ListPlaceHolder);
        }

        public override void View_Load(object sender, EventArgs e)
        {

            base.View_Load(sender, e);
        }

        public override void PartialRender()
        {

            var parameter = this.View.PostParameter;
            string action = parameter.Action;
            switch (action)
            {
                case "SaveCampaign":
                    View.AddControl(ControlPath.ICampaignsEditorView, View.ListPlaceHolder);
                    View.ListPanel.RefreshPanel(View.ListPlaceHolder, new[] { "aa;" });
                    break;
                case "EditCampaign":
                    View.AddControl(ControlPath.ICampaignsEditorView, View.ListPlaceHolder);
                    View.ListPanel.RefreshPanel(View.ListPlaceHolder, new[] { "aa;" });
                    break;
                case "SelectMedia":
                    View.AddControl(ControlPath.ICampaignsMediaSelectorView, View.ListPlaceHolder);
                    View.ListPanel.RefreshPanel(View.ListPlaceHolder, new[] { "aa;" });
                    break;
                case "ShowEditor":
                    View.AddControl(ControlPath.ICampaignsEditorView, View.ListPlaceHolder);
                    View.ListPanel.RefreshPanel(View.ListPlaceHolder, new[] { "aa;" });
                    break;
                case "SelectLayout":
                    View.AddControl(ControlPath.ICampaignsLayoutSelectorView, View.ListPlaceHolder);
                    View.ListPanel.RefreshPanel(View.ListPlaceHolder, new[] { "aa;" });
                    break;
                     
            }


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