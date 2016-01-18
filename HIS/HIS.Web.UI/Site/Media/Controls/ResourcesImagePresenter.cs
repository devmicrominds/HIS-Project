using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class ResourcesImagePresenter : GenericPresenter<IResourcesImageView>
    {
        private IRepositoryFactory factory;
        private string page_key = "resources.imagecategory";
        //private IDictionary<string, Action> RunAction;
        public ResourcesImagePresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
            // BuildRunActions();
        }

        /*private void BuildRunActions()
        {
            RunAction = new Dictionary<string, Action>() 
            {
                 
                 { "SaveImage",SaveImage },
                 
            };
        }*.

        private void SaveImage()
        {
            var p = View.PostParameter;
            var data = p.Data;


            var repository = factory.GetRepository<Repository<MediaCategory>>();
            string groupid = data.GroupID;

            var oMedCat = new MediaCategory()
            {
                Name = View.PostParameter.Data.Name,
                Description = View.PostParameter.Data.Description,
                MediaResourceType = View.PostParameter.Data.Resource,
                ColorCode = View.PostParameter.Data.Color

            };


            factory.OnTransaction(() =>
            {
                oMedCat = repository.SaveOrUpdate(oMedCat);
            });
        }*/

        public override void View_Load(object sender, EventArgs e)
        {
            if (!PageSettingsList.ContainsKey(page_key)) 
                PageSettingsList[page_key] = PageSettings.Default();

            //string action = View.PostParameter.Action;
            //if (RunAction.ContainsKey(action))
            //{
            //    RunAction[action]();
            //}

            View.Model = GetViewModel();
            //View.JsonData = Json(new
            //{
            //    TypeSelection = ResourceTypeSelection(),
            //    SelectedRole = "2"
            //});
            View.LocalPageSettings = PageSettingsList[page_key];
            View.Grid.VirtualItemCount = View.Model.ItemCount;
            View.Grid.DataSource = View.Model.Items;
            View.Grid.DataBind();

        }

        private dynamic ResourceTypeSelection()
        {
            var list = new List<object>();

            list.Add(new { title = @"Media", value = "1" });
            list.Add(new { title = @"Image", value = "2" });
            return list.ToArray();
        }

        private PagedResult<MediaCategory> GetViewModel()
        {
            var repository = factory.GetRepository<Repository<MediaCategory>>();
            var query = repository.GetQueryable();

            //query.ToList().ForEach(x =>
            //{
            //    ResourceType t = ResourceType.Image; //Image
            //    ResourceType t2 = x.MediaResourceType; //1
            //    string t3 = x.MediaResourceType.ToString();

            //});

            var query2 = query.Where(o => o.MediaResourceType == ResourceType.Image);

            return repository.PagedResult(query2, PageSettingsList[page_key].PageIndex, PageSettingsList[page_key].PageSize);
        }
    }
}